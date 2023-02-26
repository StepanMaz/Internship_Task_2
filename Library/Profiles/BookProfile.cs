using AutoMapper;
using Database;
using Database.Entities;
using DTO;

namespace Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<BookDTO, Book>();

            CreateMap<Book, ReviewedBookDTO>()
                .ForMember(dest => dest.ReviewNumber,
                    option => option.MapFrom(src => src.Reviews.Any() ? src.Reviews.Count() : 0))
                .ForMember(dest => dest.Rating,
                    option => option.MapFrom(src => src.Ratings.Any() ? Math.Round(src.Ratings.Average(r => r.Score), 1) : 0));

            CreateMap<Book, ExpandedBookDTO>()
                .ForMember(dest => dest.Rating,
                    option => option.MapFrom(src => src.Ratings.Any() ? Math.Round(src.Ratings.Average(r => r.Score)) : 0));
        }
    }
}