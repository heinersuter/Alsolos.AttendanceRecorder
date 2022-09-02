# Starting Windows service

start admin command prompt in directroy of AttendanceRecorder.WindowsService

    sc.exe create "Attendance Recorder Service 2" binPath=%cd%"\bin\Debug\AttendanceRecorder.WindowsService.exe" start=auto
    sc.exe start "Attendance Recorder Service 2"