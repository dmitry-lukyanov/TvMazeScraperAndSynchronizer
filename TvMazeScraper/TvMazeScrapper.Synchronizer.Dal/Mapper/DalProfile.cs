using AutoMapper;
using TvMazeScraper.Models;
using TvMazeScraper.Synchronizer.Dal.Dto;

namespace TvMazeScraper.Synchronizer.Dal.Mapper
{
    public class DalProfile : Profile
    {
        public DalProfile()
        {
            CreateMap<Show, ShowDto>()
                .ForMember(c => c.Cast, y => y.Ignore())
                .ReverseMap();
            CreateMap<Cast, CastDto>().ReverseMap();
        }
    }
}
