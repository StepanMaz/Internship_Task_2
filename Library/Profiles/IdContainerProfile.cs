using AutoMapper;
using Database.Entities;
using DTO;

namespace Profiles
{
    public class IdContaningProfile : Profile
    {
        public IdContaningProfile()
        {
            CreateMap<Book, IdContainerDTO>();
            CreateMap<Rating, IdContainerDTO>();
            CreateMap<Review, IdContainerDTO>();
        }
    }
}