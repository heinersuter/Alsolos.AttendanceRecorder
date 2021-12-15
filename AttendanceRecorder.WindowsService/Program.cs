using AttendanceRecorder.WindowsService;
using AttendanceRecorder.WindowsService.LifeSign;
using AttendanceRecorder.WindowsService.ServiceEvents;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(@"C:\Users\e001150\source\repos\Alsolos.AttendanceRecorder\AttendanceRecorder.WindowsService\bin\Debug\log.txt")
            .CreateLogger();

IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .UseWindowsService()
    .ConfigureServices(services =>
    {
        services.AddSingleton<LifeSignWorker, LifeSignWorker>();
        //services.AddHostedService<LifeSignWorker>();
        services.AddSingleton<IHostLifetime, CustomWindowsServiceLifetime>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
