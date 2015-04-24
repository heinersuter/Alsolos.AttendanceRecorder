namespace Alsolos.AttendanceRecorder.WindowsService
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using Alsolos.AttendanceRecorder.LocalService;

    public class LifeSignSender : IDisposable
    {
        private readonly TimeSpan _updatePeriod = new TimeSpan(0, 0, 10);
        private readonly ManualResetEvent _runEvent = new ManualResetEvent(false);
        private readonly BackgroundWorker _backgroundWorker = new BackgroundWorker();
        private readonly AttendanceRecorderService _service;
        private string _timeAccountname;

        public LifeSignSender()
        {
            _service = new AttendanceRecorderService();
            _backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
            _backgroundWorker.RunWorkerAsync();
        }

        public void Start()
        {
            if (_timeAccountname == null)
            {
                _timeAccountname = InteractiveUser.GetInteractiveUser() + "@" + Environment.MachineName;
            }
            if (_timeAccountname != null)
            {
                _runEvent.Set();
            }
        }

        public void Stop()
        {
            _runEvent.Reset();
        }

        public void Dispose()
        {
            if (_service != null)
            {
                _service.Dispose();
            }
        }

        private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            while (_backgroundWorker.IsBusy)
            {
                if (_runEvent.WaitOne())
                {
                    _service.KeepAlive(_timeAccountname, _updatePeriod);
                }
                Thread.Sleep(_updatePeriod);
            }
        }
    }
}
