@ECHO.

:: as admin 

:: delete: 
::sc.exe delete "Attendance Recorder Service"

sc.exe create "Attendance Recorder Service" binPath=%cd%"\bin\Debug\Alsolos.AttendanceRecorder.WindowsService.exe" start=auto
sc.exe description "Attendance Recorder Service" "Tracks the time a user is logged in and provides a web API."
sc.exe start "Attendance Recorder Service"

::C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /i bin\Debug\Alsolos.AttendanceRecorder.WindowsService.exe

@ECHO.
::@PAUSE