namespace Alsolos.AttendanceRecorder.WindowsService
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using Alsolos.AttendanceRecorder.LocalService;
    using Alsolos.AttendanceRecorder.WindowsService.Properties;

    public class LifeSignSender : IDisposable
    {
        private readonly TimeSpan _updatePeriod = new TimeSpan(0, 0, 30);
        private readonly ManualResetEvent _runEvent = new ManualResetEvent(false);
        private readonly BackgroundWorker _backgroundWorker = new BackgroundWorker();
        private readonly AttendanceRecorderService _service;

        public LifeSignSender()
        {
            _service = new AttendanceRecorderService();
            _backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
            _backgroundWorker.RunWorkerAsync();
        }

        public void Dispose()
        {
            if (_service != null)
            {
                _service.Dispose();
            }
        }

        public void Start()
        {
            _runEvent.Set();
        }

        public void Stop()
        {
            _runEvent.Reset();
        }

        private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            while (_backgroundWorker.IsBusy)
            {
                if (_runEvent.WaitOne())
                {
                    if (Settings.Default.UserName == InteractiveUser.GetInteractiveUser())
                    {
                        _service.KeepAlive(Settings.Default.TimeAccountName, _updatePeriod);
                    }
                }
                Thread.Sleep(_updatePeriod);
            }
        }
    }
}
