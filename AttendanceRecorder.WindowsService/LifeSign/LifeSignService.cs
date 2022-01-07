using AttendanceRecorder.WindowsService.Store;

namespace AttendanceRecorder.WindowsService.LifeSign
{
    public class LifeSignService : BackgroundService
    {
        private readonly ManualResetEvent _runEvent = new(false);
        private readonly TimeSpan _updatePeriod = TimeSpan.FromSeconds(10);

        private readonly FileStore _fileStore = new();
        private readonly ILogger<LifeSignService> _logger;

        public LifeSignService(ILogger<LifeSignService> logger)
        {
            _logger = logger;
            _logger.LogDebug("LifeSignService created");
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_runEvent.WaitOne())
                {
                    _logger.LogDebug("LifeSignService writing life sign");
                    _fileStore.WriteLifeSign();
                }

                await Task.Delay(_updatePeriod, cancellationToken);
            }
        }
    }
}
