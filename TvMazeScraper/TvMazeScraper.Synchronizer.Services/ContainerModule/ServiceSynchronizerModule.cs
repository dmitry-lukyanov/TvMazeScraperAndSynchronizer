using Autofac;

namespace TvMazeScraper.Synchronizer.Services.ContainerModule
{
    public class ServiceSynchronizerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(typeof(ServiceSynchronizerModule).Assembly).AsImplementedInterfaces();
        }
    }
}
