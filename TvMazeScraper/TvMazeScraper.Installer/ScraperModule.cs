using System.Collections.Generic;
using Autofac;
using AutoMapper;
using TvMazeScraper.Dal.ContainerModule;
using TvMazeScraper.Services.ContainerModule;

namespace TvMazeScraper.Installer
{
    public class ScraperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule<DalModule>();
            builder.RegisterModule<ServiceModule>();

            builder.Register<Mapper>(context =>
            {
                var profiles = context.Resolve<IEnumerable<Profile>>();
                Mapper.Initialize(cfg =>
                {
                    foreach (var profile in profiles)
                    {
                        cfg.AddProfile(profile);
                    }
                });
                return (Mapper)Mapper.Instance;
            }).AsImplementedInterfaces().SingleInstance().AutoActivate();
        }
    }
}
