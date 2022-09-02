using System;
using System.ComponentModel;
using System.ServiceProcess;
using System.Threading;

namespace AttendanceRecorder.WindowsService;

public partial class Service : ServiceBase
{
    private readonly BackgroundWorker _backgroundWorker = new();
    private readonly ManualResetEvent _resetEvent = new(false);
    private readonly LifeSignWriter _lifeSignWriter = new();
    private readonly TimeSpan _cyclePeriod = TimeSpan.FromSeconds(2);
    private readonly TimeSpan _updatePeriod = TimeSpan.FromSeconds(30);
        
    public Service()
    {
        InitializeComponent();
        ServiceName = "New Attendance Recorder Service";
        CanHandleSessionChangeEvent = true;

        _backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
        _backgroundWorker.RunWorkerAsync();
    }

    protected override void OnStart(string[] args)
    {
        _resetEvent.Set();
    }

    protected override void OnStop()
    {
        _resetEvent.Reset();
    }

    private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
    {
        var lastLifeSignWritten = DateTime.MinValue;

        while (_backgroundWorker.IsBusy)
        {
            if (_resetEvent.WaitOne() && lastLifeSignWritten + _updatePeriod < DateTime.Now)
            {
                _lifeSignWriter.WriteLifeSign();
                lastLifeSignWritten = DateTime.Now;
            }

            Thread.Sleep(_cyclePeriod);
        }
    }

    protected override void OnSessionChange(SessionChangeDescription changeDescription)
    {
        switch (changeDescription.Reason)
        {
            case SessionChangeReason.SessionLock:
            case SessionChangeReason.SessionLogoff:
                _resetEvent.Reset();
                break;
            case SessionChangeReason.SessionUnlock:
            case SessionChangeReason.SessionLogon:
                _resetEvent.Set();
                break;
        }
    }
}