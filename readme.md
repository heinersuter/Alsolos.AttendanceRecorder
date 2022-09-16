# Building and installing the application

## Prerequisits

* node.js

## Building the frontend

From directory `attendance-recorder.web-ui` execute

    npm install
    npm run build

UI is build and output is copied to `wwwroot` directory of project `AttendanceRecorder.Webhost`.

## Building the backends

## Insatlling all windows services

* API

* UI Host

* Windows Service


## Starting Windows service

Start admin terminal (powershell) in directroy of AttendanceRecorder.WindowsService

    sc.exe create "Attendance Recorder Service 2" binPath=$pwd."\bin\Debug\AttendanceRecorder.WindowsService.exe" start=auto
    sc.exe start "Attendance Recorder Service 2"

    sc.exe create "Attendance Recorder API" binPath=$pwd."\bin\Debug\net6.0\AttendanceRecorder.WebApi.exe" start=auto

