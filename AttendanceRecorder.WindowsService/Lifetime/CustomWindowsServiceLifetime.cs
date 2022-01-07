using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Options;
using System.ServiceProcess;

namespace AttendanceRecorder.WindowsService.Lifetime
{
    // https://stackoverflow.com/questions/65128031/net-core-3-service-service-manager-wont-wait-for-service-startup
    public sealed class CustomWindowsServiceLifetime : WindowsServiceLifetime
    {
        private readonly ILogger<CustomWindowsServiceLifetime> _logger;

        public CustomWindowsServiceLifetime(
            IHostEnvironment environment,
            IHostApplicationLifetime applicationLifetime,
            ILoggerFactory loggerFactory,
            IOptions<HostOptions> optionsAccessor            , 
            ILogger<CustomWindowsServiceLifetime> logger)
            : base(environment, applicationLifetime, loggerFactory, optionsAccessor)
        {
            _logger = logger;
            _logger.LogDebug("WindowsServiceLifetime created");
            CanHandleSessionChangeEvent = true;
        }

        public event EventHandler Activated = delegate { };

        public event EventHandler Deactivated = delegate { };


        protected override void OnStart(string[] args)
        {
            _logger.LogDebug("WindowsServiceLifetime started");
            base.OnStart(args);
            Activated.Invoke(this, EventArgs.Empty);
        }

        protected override void OnStop()
        {
            Deactivated.Invoke(this, EventArgs.Empty);
            base.OnStop();
            _logger.LogDebug("WindowsServiceLifetime stopped");
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            _logger.LogDebug($"WindowsServiceLifetime session changed to '{changeDescription.Reason}'");
            base.OnSessionChange(changeDescription);

            if (changeDescription.Reason == SessionChangeReason.SessionUnlock
                || changeDescription.Reason == SessionChangeReason.SessionLogon)
            {
                Activated.Invoke(this, EventArgs.Empty);
            }
            else if (changeDescription.Reason == SessionChangeReason.SessionLock
                || changeDescription.Reason == SessionChangeReason.SessionLogoff)
            {
                Deactivated.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
