using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TvMazeScraper.Api.Synchronizer.HostServices
{
    public abstract class TimerHostService : IHostedService, IDisposable
    {
        protected Timer Timer;
        protected readonly ILogger<TimerHostService> Logger;

        protected TimerHostService(ILogger<TimerHostService> logger)
        {
            Logger = logger;
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            if (!CanBeCalled) return Task.CompletedTask;

            cancellationToken.Register(() =>
            {
                StopProcessAsync();
            });

            Logger.LogInformation($"{HostName} is started");

            Timer = new Timer(async (state) =>
            {
                Logger.LogInformation($"{HostName} is timer event started");
                await OnTimerCalledAsync(cancellationToken);
                Logger.LogInformation($"{HostName} is timer event finished");
            }, null, Duetime, Period);

            return Task.CompletedTask;
        }

        public abstract Task OnTimerCalledAsync(CancellationToken cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return StopProcessAsync();
        }

        public virtual bool CanBeCalled => true;

        private Task StopProcessAsync()
        {
            Logger.LogInformation($"{HostName} is stopped");

            Timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public abstract string HostName { get; }

        public abstract TimeSpan Period { get; }

        public virtual TimeSpan Duetime => TimeSpan.Zero;

        public void Dispose()
        {
            Timer?.Dispose();
        }
    }
}
