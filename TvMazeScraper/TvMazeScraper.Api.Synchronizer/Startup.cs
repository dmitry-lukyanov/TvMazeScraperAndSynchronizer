using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using TvMazeScraper.Api.Synchronizer.HostServices;
using TvMazeScraper.Api.Synchronizer.Utils;
using TvMazeScraper.Synchronizer.Installer;
using TvMazeScraper.Utils.Extensions;

namespace TvMazeScraper.Api.Synchronizer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddHttpClient();
            services.ConfigureLogging();
            services.AddHostedService<FullSynchronizeScraperHostService>();
            services.AddHostedService<LastUpdatesSynchronizeScraperHostService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Tv Maze Scraper Proxy", Version = "v1" });
            });

            var builder = new ContainerBuilder();
            builder.RegisterModule<ScraperSynchronizerModule>();
            builder.RegisterType<ApiSettingsProvider>().AsImplementedInterfaces();
            builder.RegisterType<TimeProvider>().AsImplementedInterfaces();
            builder.Populate(services);

            var applicationContainer = builder.Build();

            return new AutofacServiceProvider(applicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.ConfigureExceptionHandler(env, Log.Logger);

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "Tv Maze Scraper Proxy V1");
            });
        }
    }
}
