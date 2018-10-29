using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Synchronizer.Dal.Context;
using TvMazeScraper.Synchronizer.Dal.Mapper;

namespace TvMazeScraper.Synchronizer.Dal.ContainerModule
{
    public class DalModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(typeof(DalModule).Assembly).AsImplementedInterfaces();

            builder.RegisterType<TvMazeScraperContext>().As<DbContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<DalProfile>().As<Profile>();
        }
    }
}
