using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Options;
using System.ServiceProcess;

namespace AttendanceRecorder.Service.LifeSign
{
    // https://stackoverflow.com/questions/65128031/net-core-3-service-service-manager-wont-wait-for-service-startup
    public sealed class CustomWindowsServiceLifetime : WindowsServiceLifetime
    {
        private readonly LifeSignWorker _lifeSignWorker;

        public CustomWindowsServiceLifetime(
            IHostEnvironment environment,
            IHostApplicationLifetime applicationLifetime,
            ILoggerFactory loggerFactory,
            IOptions<HostOptions> optionsAccessor,
            LifeSignWorker lifeSignWorker)
            : base(environment, applicationLifetime, loggerFactory, optionsAccessor)
        {
            _lifeSignWorker = lifeSignWorker;
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            _lifeSignWorker.StartAsync(CancellationToken.None).Wait();
        }

        protected override void OnStop()
        {
            _lifeSignWorker.StopAsync(CancellationToken.None).Wait();
            base.OnStop();
        }

        protected override void OnPause()
        {
            _lifeSignWorker.StopAsync(CancellationToken.None).Wait();
            base.OnPause();
        }

        protected override void OnContinue()
        {
            base.OnContinue();
            _lifeSignWorker.StartAsync(CancellationToken.None).Wait();
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
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
