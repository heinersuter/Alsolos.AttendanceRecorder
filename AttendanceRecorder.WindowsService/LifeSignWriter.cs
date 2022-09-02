using System;
using System.IO;

namespace AttendanceRecorder.WindowsService;

internal class LifeSignWriter
{
    public LifeSignWriter()
    {
        if (!Directory.Exists(Settings.Default.LocalDirectory))
        {
            Directory.CreateDirectory(Settings.Default.LocalDirectory);
        }
    }

    public void WriteLifeSign()
    {
        var now = DateTime.Now;

        var directory = Path.Combine(Settings.Default.LocalDirectory, $"{now.Year}", $"{now.Month:D2}");
        var file = Path.Combine(directory, $"{now.Day:D2}.att");

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.AppendAllText(file, $"{now:HH:mm:ss}{Environment.NewLine}");
    }
}
