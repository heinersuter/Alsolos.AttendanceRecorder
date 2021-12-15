using AttendanceRecorder.Service.LifeSign;
using Microsoft.Extensions.Hosting.WindowsServices;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.File(@"C:\Users\e001150\source\repos\Alsolos.AttendanceRecorder\AttendanceRecorder.Service\bin\Debug\store\log.txt")
            .CreateLogger();

var options = new WebApplicationOptions
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default,
};

var builder = WebApplication.CreateBuilder(options);

//if (!Environment.UserInteractive)
//{
//    builder.Services.AddSingleton<IHostLifetime, CustomWindowsServiceLifetime>();
//}

builder.Host.ConfigureServices(services => services.AddSingleton<IHostLifetime, CustomWindowsServiceLifetime>());
builder.Host.UseWindowsService();
builder.Host.UseSerilog();
builder.WebHost.UseUrls("http://localhost:5005");

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<LifeSignWorker>(new LifeSignWorker());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

Log.Logger.Information("Application will be started");

app.Run();
