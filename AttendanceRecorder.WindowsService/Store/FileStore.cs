namespace AttendanceRecorder.WindowsService.Store
{
    public class FileStore
    {
        public FileStore()
        {
            if (!Directory.Exists(LocalDirectory))
            {
                Directory.CreateDirectory(LocalDirectory);
            }
        }

        public string LocalDirectory { get; } = @"C:\Users\e001150\source\repos\AttendanceRecorder\AttendanceRecorder.WindowsService\bin\Debug\store";

        public void WriteLifeSign()
        {
            var lifeSign = DateTime.Now;
            lifeSign = lifeSign.AddTicks(-(lifeSign.Ticks % TimeSpan.TicksPerSecond));

            var filePath = Path.Combine(LocalDirectory, $"{lifeSign:yyyy-MM-dd}.ar");
            File.AppendAllText(filePath, lifeSign.ToString("HH:mm:sszzz") + Environment.NewLine);
        }
    }
}
