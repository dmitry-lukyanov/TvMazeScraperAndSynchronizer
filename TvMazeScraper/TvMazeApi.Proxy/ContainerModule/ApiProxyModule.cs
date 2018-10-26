using Autofac;
using AutoMapper;
using TvMazeApi.Proxy.Mapper;

namespace TvMazeApi.Proxy.ContainerModule
{
    public class ApiProxyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(typeof(ApiProxyModule).Assembly).AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ApiProxyProfile>().As<Profile>();
        }
    }
}
