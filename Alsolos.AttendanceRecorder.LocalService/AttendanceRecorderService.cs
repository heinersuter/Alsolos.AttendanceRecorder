using System;
using Alsolos.AttendanceRecorder.WebApi;

namespace Alsolos.AttendanceRecorder.LocalService
{
    public class AttendanceRecorderService : IDisposable
    {
        private readonly LocalFileSystemStore _fileSystemStore = new LocalFileSystemStore();
        private readonly WebApiStarter _webApiStarter;

        public AttendanceRecorderService()
        {
            var intervalAggregator = new IntervalCollection(_fileSystemStore);
            _webApiStarter = new WebApiStarter(intervalAggregator, _fileSystemStore);
            _webApiStarter.Start();
        }

        public void KeepAlive(string timeAccountName, TimeSpan updatePeriod)
        {
            _fileSystemStore.SaveLifeSign();
        }

        public void Dispose()
        {
            if (_webApiStarter != null)
            {
                _webApiStarter.Dispose();
            }
        }
    }
}
