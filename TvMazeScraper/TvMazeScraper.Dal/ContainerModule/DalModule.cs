using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Dal.Context;
using TvMazeScraper.Dal.Mapper;

namespace TvMazeScraper.Dal.ContainerModule
{
    public class DalModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(typeof(DalModule).Assembly)
                .AsImplementedInterfaces();

            builder.RegisterType<DalProfile>().As<Profile>();
            builder.RegisterType<TvMazeScraperContext>().As<DbContext>().AsSelf().SingleInstance();
        }
    }
}
