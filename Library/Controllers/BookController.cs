using DTO;
using Validators;
using Repositories;
using Configuration;
using Database.Entities;

using AutoMapper;
using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Controllers
{
    [Route("api")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IConfig configuration;
        private ILogger<BookController> logger;

        private IGenericRepository<Book> books;
        private IGenericRepository<Review> reviews;
        private IGenericRepository<Rating> ratings;
        private IMapper mapper;

        public BookController(IConfig configuration,
                              ILogger<BookController> logger,
                              IGenericRepository<Book> books,
                              IGenericRepository<Review> reviews,
                              IGenericRepository<Rating> ratings,
                              IMapper mapper)
        {
            this.logger = logger;
            this.books = books;
            this.reviews = reviews;
            this.ratings = ratings;
            this.mapper = mapper;
        }

        [HttpGet("books")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAll([FromQuery(Name = "order")] string order)
        {
            if(order is null)
            {
                return Ok((await books.GetAll()).Select(mapper.Map<BookDTO>));
            }
            else 
            {
                var field = typeof(Book).GetProperty(order, System.Reflection.BindingFlags.IgnoreCase);

                if(field is null)
                {
                    return BadRequest("Provided `order` is not one of the valid choices.");
                }
                else
                {
                    return Ok((await books.GetAll()).OrderBy(book => field.GetValue(book)).Select(mapper.Map<BookDTO>));
                }
            }
        }

        [HttpGet("recommended")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetTopRatedBooks([FromQuery(Name = "genre")] string genre)
        {
            var query = books.Get();
                        
            if(genre is string)
            {
                query.Where(book => book.Genere == genre);
            }

            query = (from book in query
                    where book.Ratings.Count() > 10
                    orderby book.Ratings.Average(rating => rating.Score)
                    select book).Take(10);

            return Ok((await query.ToListAsync()).Select(mapper.Map<BookDTO>));
        }

        [HttpGet("books/{id:int}")]
        public ActionResult<IEnumerable<ExpandedBookDTO>> GetBookWithRviews(int id)
        {
            var book = books.FindById(id);
            if(book is null) {
                return NotFound($"Book with id = {id} was not found");
            }
            return Ok(mapper.Map<ExpandedBookDTO>(book));
        }

        
        [HttpDelete("books/{id:int}?{secret}")]
        public async Task<ActionResult> GetBookWithRviews(int id, string secret)
        {
            if(configuration.IsValidSecret(secret))
            {
                await books.Remove(await books.FindById(id));
                return Ok();
            }

            return BadRequest("Secret is not valid");
        }

        
        [HttpPost("books/save")]
        public async Task<ActionResult> SaveBook([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] BookDTO bookDTO)
        {
            var book = mapper.Map<Book>(bookDTO);

            await new BookValidator().ValidateAndThrowAsync(book);

            if(bookDTO.Id is not null)
            {
                await books.Create(book); 
            }
            else
            {
                await books.Update(book);
            }
             return Ok(mapper.Map<IdContainerDTO>(book));
        }

         [HttpPut("books/{id:int}/review")]
        public async Task<ActionResult> AddReview([FromQuery(Name = "id")] int id,
                                                  [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] ReviewDTO reviewDTO)
        {
            var review = mapper.Map<Review>(reviewDTO);
            review.BookID = id;

            await new ReviewValidator().ValidateAndThrowAsync(review);

            await reviews.Create(review);

            return Ok(mapper.Map<IdContainerDTO>(review));
        }

        [HttpPut("books/{id:int}/rate")]
        public async Task<ActionResult> RateABook([FromQuery(Name = "id")] int id,
                                                  [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] RatingDTO ratingDTO)
        {
            var rating = mapper.Map<Rating>(ratingDTO);
            rating.BookID = id;

            await new RatingValidator().ValidateAndThrowAsync(rating);

            await ratings.Create(rating);

            return Ok(mapper.Map<IdContainerDTO>(rating));
        }
    }
}