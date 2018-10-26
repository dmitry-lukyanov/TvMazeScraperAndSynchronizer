using AutoMapper;
using TvMazeScraper.Dal.Dto;
using TvMazeScraper.Models;

namespace TvMazeScraper.Dal.Mapper
{
    public class DalProfile : Profile
    {
        public DalProfile()
        {
            CreateMap<Show, ShowDto>().ReverseMap();
            CreateMap<Cast, CastDto>().ReverseMap();
        }
    }
}
