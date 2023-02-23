using AutoMapper;
using Database.Entities;
using DTO;

namespace Profiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<ReviewDTO, Review>();
            CreateMap<Review, ExpandedBookDTO.ExpandedBookDTOReviewDTO>();
        }
    }
}