using System.Linq;
using AutoMapper;
using TvMazeApi.Proxy.Dto;
using TvMazeScraper.Models;

namespace TvMazeApi.Proxy.Mapper
{
    public class ApiProxyProfile : Profile
    {
        public ApiProxyProfile()
        {
            CreateMap<ShowDto, Show>()
                .ForMember(c => c.Cast, y => y.Ignore())
                .AfterMap((s, d, context) =>
                {
                    var persons = s?.Embedded?.Cast?.Select(c => c.Person)?.ToList();
                    if (persons != null && persons.Any())
                    {
                        d.Cast = persons.Select(c =>
                        {
                            var cast = context.Mapper.Map<PersonDto, Cast>(c);
                            cast.ShowId = s.Id;
                            return cast;
                        });
                    }
                });
            CreateMap<PersonDto, Cast>();
        }
    }
}
