namespace Alsolos.AttendanceRecorder.WindowsService
{
    using System;
    using System.ServiceProcess;

    public static class Program
    {
        public static void Main()
        {
            if (!Environment.UserInteractive)
            {
                var servicesToRun = new ServiceBase[] { new AttendanceRecorderWindowsService(), };
                ServiceBase.Run(servicesToRun);
            }
            else
            {
                var worker = new LifeSignSender();
                worker.Start();

                var key = (char)Console.Read();
                while (key != 'e')
                {
                    if (key == 's')
                    {
                        Console.WriteLine("Start");
                        worker.Start();
                    }
                    if (key == 'p')
                    {
                        Console.WriteLine("Stop");
                        worker.Stop();
                    }
                    key = (char)Console.Read();
                }
            }
        }
    }
}
