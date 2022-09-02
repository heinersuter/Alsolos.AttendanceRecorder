using System.ServiceProcess;

namespace AttendanceRecorder.WindowsService;

internal static class Program
{
    private static void Main()
    {
        ServiceBase.Run(new ServiceBase[]
        {
                new Service(),
        });
    }
}
