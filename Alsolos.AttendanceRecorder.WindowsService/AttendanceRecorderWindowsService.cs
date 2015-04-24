namespace Alsolos.AttendanceRecorder.WindowsService
{
    using System.ServiceProcess;

    public class AttendanceRecorderWindowsService : ServiceBase
    {
        private readonly LifeSignSender _lifeSignSender;

        public AttendanceRecorderWindowsService()
        {
            ServiceName = "Attendance Recorder Service";

            CanHandleSessionChangeEvent = true;
            CanPauseAndContinue = true;
            CanShutdown = true;
            CanStop = true;
            _lifeSignSender = new LifeSignSender();
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            _lifeSignSender.Start();
        }

        protected override void OnStop()
        {
            base.OnStop();
            _lifeSignSender.Stop();
        }

        protected override void OnPause()
        {
            base.OnPause();
            _lifeSignSender.Stop();
        }

        protected override void OnContinue()
        {
            base.OnContinue();
            _lifeSignSender.Start();
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();
            _lifeSignSender.Stop();
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            base.OnSessionChange(changeDescription);
            if (changeDescription.Reason == SessionChangeReason.SessionLock
                || changeDescription.Reason == SessionChangeReason.SessionLogoff)
            {
                _lifeSignSender.Stop();
            }
            else if (changeDescription.Reason == SessionChangeReason.SessionUnlock
                || changeDescription.Reason == SessionChangeReason.SessionLogon)
            {
                _lifeSignSender.Start();
            }
        }
    }
}
