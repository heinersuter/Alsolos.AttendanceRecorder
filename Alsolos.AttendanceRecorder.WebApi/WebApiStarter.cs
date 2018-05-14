namespace Alsolos.AttendanceRecorder.WebApi
{
    using System;
    using Alsolos.AttendanceRecorder.WebApi.Model;
    using Microsoft.Owin.Hosting;

    public class WebApiStarter : IDisposable
    {
        private IDisposable _webApiService;

        public WebApiStarter(IIntervalCollection intervalCollection)
        {
            IntervalCollection = intervalCollection;
        }

        public static IIntervalCollection IntervalCollection { get; private set; }

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
