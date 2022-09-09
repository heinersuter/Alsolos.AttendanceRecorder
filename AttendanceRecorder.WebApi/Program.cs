using AttendanceRecorder.WebApi.FileSystem;
using AttendanceRecorder.WebApi.Periods;
using AttendanceRecorder.WebApi.Weeks;
using Microsoft.Extensions.Hosting.WindowsServices;
using AttendanceRecorder.WebApi.FrameworkExtensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

TypeDescriptor.AddAttributes(typeof(DateOnly), new TypeConverterAttribute(typeof(DateOnlyTypeConverter)));
TypeDescriptor.AddAttributes(typeof(TimeOnly), new TypeConverterAttribute(typeof(TimeOnlyTypeConverter)));

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default,
});
builder.Host.UseWindowsService();

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
    options.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter());
});

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
