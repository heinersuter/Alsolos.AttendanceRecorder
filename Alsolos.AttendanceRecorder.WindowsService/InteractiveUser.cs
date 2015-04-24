namespace Alsolos.AttendanceRecorder.WindowsService
{
    using System;
    using System.Diagnostics;
    using System.Management;

    public static class InteractiveUser
    {
        public static string GetInteractiveUser()
        {
            foreach (var process in Process.GetProcesses())
            {
                if (process.ProcessName == "explorer")
                {
                    return GetProcessOwner(process.Id);
                }
            }
            return null;
        }

        public static string GetProcessOwner(int processId)
        {
            var query = "Select * From Win32_Process Where ProcessID = " + processId;
            var searcher = new ManagementObjectSearcher(query);
            var processList = searcher.Get();

            foreach (var processObject in processList)
            {
                var obj = (ManagementObject)processObject;
                object[] argList = { string.Empty, string.Empty };
                var returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return DOMAIN\user
                    return argList[1] + "\\" + argList[0];
                }
            }
            return "NO OWNER";
        }
    }
}
