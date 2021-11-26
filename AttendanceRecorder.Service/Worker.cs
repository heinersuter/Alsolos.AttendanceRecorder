namespace AttendanceRecorder.Service
{
    public class Worker : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // simulate loading... non of this cause wait
            Thread.Sleep(5000);
            await Task.Delay(5000);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(1);
        }
    }
}
