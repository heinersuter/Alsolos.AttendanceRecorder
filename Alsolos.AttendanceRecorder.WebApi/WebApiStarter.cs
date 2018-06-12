using Alsolos.AttendanceRecorder.WebApi.FileSystem;
using Alsolos.AttendanceRecorder.WebApi.Intervals;

namespace Alsolos.AttendanceRecorder.WebApi
{
    using System;
    using Microsoft.Owin.Hosting;

    public class WebApiStarter : IDisposable
    {
        private IDisposable _webApiService;

        public WebApiStarter(IIntervalCollection intervalCollection, IFileSystemStore fileSystemStore)
        {
            IntervalCollection = intervalCollection;
            FileSystemStore = fileSystemStore;
        }

        public static IIntervalCollection IntervalCollection { get; private set; }

        public static IFileSystemStore FileSystemStore { get; private set; }

        public void Start()
        {
            _webApiService = WebApp.Start<Startup>("http://localhost:30515/");
        }

        public void Dispose()
        {
            if (_webApiService != null)
            {
                _webApiService.Dispose();
            }
        }
    }
}
