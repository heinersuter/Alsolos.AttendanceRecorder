using AttendanceRecorder.WindowsService.LifeSign;
using AttendanceRecorder.WindowsService.Lifetime;
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
    .UseWindowsService()
    .UseSerilog(logger:Log.Logger)
    .ConfigureServices(services =>
    {
        services.AddSingleton<LifeSignService>();
        services.AddSingleton<IHostLifetime, CustomWindowsServiceLifetime>();
    })
    .Build();

System.ServiceProcess.ServiceBase.Run(new LifetimeService(host.Services.GetRequiredService<Microsoft.Extensions.Logging.ILogger<LifetimeService>>()));

await host.RunAsync();
