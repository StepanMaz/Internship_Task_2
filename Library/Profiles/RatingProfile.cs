using AutoMapper;
using Database.Entities;
using DTO;

namespace Profiles
{
    public class RatingProfile : Profile
    {
        public RatingProfile()
        {
            CreateMap<RatingDTO, Rating>();
        }
    }
}