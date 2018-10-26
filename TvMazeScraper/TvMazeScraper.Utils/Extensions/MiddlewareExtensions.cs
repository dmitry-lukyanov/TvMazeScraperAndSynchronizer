using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Serilog;
using TvMazeScraper.Utils.ErrorHandling;

namespace TvMazeScraper.Utils.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IHostingEnvironment environment, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {

                        var message = "Internal Server" + (environment.IsDevelopment() ? " " + contextFeature.Error.GetBaseException().Message : string.Empty);
                        logger.Fatal(contextFeature.Error, "Unhandled error");

                        await context.Response.WriteAsync(new ErrorMessage()
                        {
                            HttpCode = context.Response.StatusCode,
                            Message = message
                        }.ToString());
                    }
                });
            });
        }
    }
}
