using AttendanceRecorder.WebApi;
using AttendanceRecorder.WebApi.FileSystem;
using AttendanceRecorder.WebApi.Periods;
using AttendanceRecorder.WebApi.Weeks;
using Microsoft.Extensions.Hosting.WindowsServices;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default,
});
builder.Host.UseWindowsService();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<WeeksService>();
builder.Services.AddScoped<PeriodService>();

builder.Services.Configure<FileSystemOptions>(builder.Configuration.GetSection("FileSystemOptions"));

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
