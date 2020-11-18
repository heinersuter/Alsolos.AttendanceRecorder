using System;
using Alsolos.AttendanceRecorder.WebApi;

namespace Alsolos.AttendanceRecorder.LocalService
{
    public class AttendanceRecorderService : IDisposable
    {
        private readonly LocalFileSystemStore _fileSystemStore;
        private readonly WebApiStarter _webApiStarter;

        public AttendanceRecorderService(string localDirectory)
        {
            _fileSystemStore = new LocalFileSystemStore(localDirectory);
            var intervalAggregator = new IntervalCollection(_fileSystemStore);
            _webApiStarter = new WebApiStarter(intervalAggregator, _fileSystemStore);
            _webApiStarter.Start();
        }

        public void KeepAlive()
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
