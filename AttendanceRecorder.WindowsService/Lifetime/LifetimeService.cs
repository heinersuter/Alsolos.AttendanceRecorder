using System.ServiceProcess;

namespace AttendanceRecorder.WindowsService.Lifetime
{
    internal class LifetimeService : ServiceBase
    {
        private readonly ILogger<LifetimeService> _logger;

        public LifetimeService(ILogger<LifetimeService> logger)
        {
            _logger = logger;
            _logger.LogDebug("LifetimeService created");
        }

        public event EventHandler Activated = delegate { };

        public event EventHandler Deactivated = delegate { };

        protected override void OnStart(string[] args)
        {
            _logger.LogDebug("LifetimeService started");
            base.OnStart(args);
            Activated.Invoke(this, EventArgs.Empty);
        }

        protected override void OnStop()
        {
            Deactivated.Invoke(this, EventArgs.Empty);
            base.OnStop();
            _logger.LogDebug("LifetimeService stopped");
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            _logger.LogDebug($"LifetimeService session changed to '{changeDescription.Reason}'");
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
