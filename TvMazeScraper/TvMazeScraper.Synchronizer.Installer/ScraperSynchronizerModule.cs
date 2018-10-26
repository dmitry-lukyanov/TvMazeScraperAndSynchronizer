using System.Collections.Generic;
using Autofac;
using AutoMapper;
using TvMazeApi.Proxy.ContainerModule;
using TvMazeScraper.Synchronizer.Dal.ContainerModule;
using TvMazeScraper.Synchronizer.Services.ContainerModule;

namespace TvMazeScraper.Synchronizer.Installer
{
    public class ScraperSynchronizerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule<DalModule>();
            builder.RegisterModule<ServiceSynchronizerModule>();
            builder.RegisterModule<ApiProxyModule>();

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
