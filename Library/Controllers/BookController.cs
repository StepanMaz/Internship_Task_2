using DTO;
using Repositories;
using Database.Entities;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection;

namespace Controllers
{
    [Route("api")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<BookController> logger;
        private readonly IGenericRepository<Book> books;
        private readonly IMapper mapper;

        public BookController(IConfiguration configuration,
                              ILogger<BookController> logger,
                              IGenericRepository<Book> books,
                              IMapper mapper)
        {
            this.logger = logger;
            this.books = books;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpGet("books")]
        public async Task<ActionResult<IEnumerable<ReviewedBookDTO>>> GetAll([FromQuery(Name = "order")] string order)
        {
            PropertyInfo property = null;
            if(order is not null)
            {
                property = typeof(ReviewedBookDTO).GetProperty(order, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (property is null)
                {
                    return BadRequest("Provided `order` is not one of the valid choices.");
                }
            }

            var bookscollection = await books.Get().ToListAsync();
            var collection = bookscollection.Select(mapper.Map<ReviewedBookDTO>);

            if (property is not null)
            {
                if(property.PropertyType == typeof(int))
                {
                    collection = collection.OrderByDescending(book => property.GetValue(book));
                }
                else
                {
                    collection = collection.OrderBy(book => property.GetValue(book));
                }
            }

            return Ok(collection);
        }

        [HttpGet("recommended")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetTopRatedBooks([FromQuery(Name = "genre")] string genre)
        {
            var query = books.Get();
                        
            if(genre is string)
            {
                query = query.Where(book => book.Genere.ToLower() == genre.ToLower());
            }

            query = (from book in query
                    where book.Ratings.Count() > 10
                    orderby book.Ratings.Average(rating => rating.Score) descending
                    select book).Take(10);

            return Ok((await query.ToListAsync()).Select(mapper.Map<ReviewedBookDTO>));
        }

        [HttpGet("books/{id:int}")]
        public async Task<ActionResult<ExpandedBookDTO>> GetBookWithReviews(int id)
        {
            var book = await books.FindById(id);
            if(book is null) {
                return NotFound($"Book with id = {id} was not found");
            }
            return Ok(mapper.Map<ExpandedBookDTO>(book));
        }

        
        [HttpDelete("books/{id:int}")]
        public async Task<ActionResult> GetBookWithReviews(int id, [FromQuery(Name = "secret")] string secret)
        {
            if(configuration.GetValue<string>("secret") == secret)
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

            if(bookDTO.Id is null)
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
        public async Task<ActionResult> AddReview(int id,
                                                  [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] ReviewDTO reviewDTO)
        {
            var book = await books.FindById(id);

            if (book is null)
            {
                return BadRequest("Wrong book id");
            }

            var review = mapper.Map<Review>(reviewDTO);

            book.Reviews.Add(review);

            await books.Update(book);

            return Ok(mapper.Map<IdContainerDTO>(review));
        }

        [HttpPut("books/{id:int}/rate")]
        public async Task<ActionResult> RateABook(int id,
                                                  [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] RatingDTO ratingDTO)
        {
            var book = await books.FindById(id);

            if (book is null)
            {
                return BadRequest("Wrong book id");
            }

            var rating = mapper.Map<Rating>(ratingDTO);

            book.Ratings.Add(rating);

            await books.Update(book);

            return Ok(mapper.Map<IdContainerDTO>(rating));
        }
    }
}