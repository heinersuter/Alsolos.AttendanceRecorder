using AttendanceRecorder.WindowsService.LifeSign;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Options;
using System.ServiceProcess;

namespace AttendanceRecorder.WindowsService.ServiceEvents
{
    // https://stackoverflow.com/questions/65128031/net-core-3-service-service-manager-wont-wait-for-service-startup
    public sealed class CustomWindowsServiceLifetime : WindowsServiceLifetime
    {
        private readonly ILogger _logger;
        private readonly LifeSignWorker _lifeSignWorker;

        public CustomWindowsServiceLifetime(
            IHostEnvironment environment,
            IHostApplicationLifetime applicationLifetime,
            ILoggerFactory loggerFactory,
            IOptions<HostOptions> optionsAccessor,
            LifeSignWorker lifeSignWorker)
            : base(environment, applicationLifetime, loggerFactory, optionsAccessor)
        {
            _logger = loggerFactory.CreateLogger<CustomWindowsServiceLifetime>();
            _lifeSignWorker = lifeSignWorker;
            _logger.LogInformation("WindowsServiceLifetime created");
        }

        protected override void OnStart(string[] args)
        {
            _logger.LogInformation("WindowsServiceLifetime OnStart");
            base.OnStart(args);
            _lifeSignWorker.StartAsync(CancellationToken.None).Wait();
        }

        protected override void OnStop()
        {
            _logger.LogInformation("WindowsServiceLifetime OnStop");
            _lifeSignWorker.StopAsync(CancellationToken.None).Wait();
            base.OnStop();
        }

        protected override void OnPause()
        {
            _logger.LogInformation("WindowsServiceLifetime OnPause");
            _lifeSignWorker.StopAsync(CancellationToken.None).Wait();
            base.OnPause();
        }

        protected override void OnContinue()
        {
            _logger.LogInformation("WindowsServiceLifetime OnContinue");
            base.OnContinue();
            _lifeSignWorker.StartAsync(CancellationToken.None).Wait();
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            _logger.LogInformation("WindowsServiceLifetime Session changed");

            base.OnSessionChange(changeDescription);
            if (changeDescription.Reason == SessionChangeReason.SessionLock
                || changeDescription.Reason == SessionChangeReason.SessionLogoff)
            {
                _lifeSignWorker.StopAsync(CancellationToken.None).Wait();
            }
            else if (changeDescription.Reason == SessionChangeReason.SessionUnlock
                || changeDescription.Reason == SessionChangeReason.SessionLogon)
            {
                _lifeSignWorker.StartAsync(CancellationToken.None).Wait();
            }
        }
    }
}
