using System;
using System.ComponentModel;
using System.Threading;
using Alsolos.AttendanceRecorder.LocalService;

namespace Alsolos.AttendanceRecorder.WindowsService
{
    public class LifeSignSender : IDisposable
    {
        private readonly BackgroundWorker _backgroundWorker = new BackgroundWorker();
        private readonly ManualResetEvent _runEvent = new ManualResetEvent(false);
        private readonly AttendanceRecorderService _service;
        private readonly TimeSpan _updatePeriod = new TimeSpan(0, 0, 30);

        public LifeSignSender()
        {
            _service = new AttendanceRecorderService(Settings.Default.LocalDirectory);
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
                    _service.KeepAlive();
                }

                Thread.Sleep(_updatePeriod);
            }
        }
    }
}