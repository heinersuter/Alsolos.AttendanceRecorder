﻿using AttendanceRecorder.WindowsService.Store;
using System.ComponentModel;

namespace AttendanceRecorder.WindowsService.LifeSign
{
    public class LifeSignWorker : IHostedService
    {
        private readonly BackgroundWorker _backgroundWorker = new BackgroundWorker();
        private readonly ManualResetEvent _runEvent = new ManualResetEvent(false);
        private readonly TimeSpan _updatePeriod = new TimeSpan(0, 0, 10);

        private FileStore _fileStore = new FileStore();

        public LifeSignWorker()
        {
            _backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
            _backgroundWorker.RunWorkerAsync();
            _runEvent.Set();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => _runEvent.Set());
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => _runEvent.Reset());
        }

        private void BackgroundWorkerOnDoWork(object? sender, DoWorkEventArgs doWorkEventArgs)
        {
            while (_backgroundWorker.IsBusy)
            {
                if (_runEvent.WaitOne())
                {
                    _fileStore.SaveLifeSign();
                }

                Thread.Sleep(_updatePeriod);
            }
        }
    }
}
