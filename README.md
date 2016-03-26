## Raspicam
# General
This is a WCF http webservice used to remotecontrol Raspberry Pis with Cameras.

# API
The API contains all Data used by WCF (Service abstraction, Data Contracts...)

# Server
The camera interface is realized by abstraction of all "raspistill" parameters and piping the camera output to the Webservice using stdout redirection.

# Client
The client is a C#6 WPF MVVM application with GUI to control up to 4 Raspberry Pis.

Note:
Set IP of the Pi(s) in App.config and start the server on the Pi(s).

Features:
- Camera setup
- Remote capture
- Save captured images

# MVVMBase/Interactivity
Classlibs providing MVVM base classes and Binding Helpers

# Compatibility

Server tested with Mono 3.2.8 on Raspberry Pi 2/3
Client Tested with .net 4.6 on Windows 10

# ToDo
- Preprocessing of the image on the Raspberry Pi
- Display Calibration Grid
- Postprocessing of the image 
- OpenCV/EmguCV camera access to improve performance
- Webservice for ProjectorControl
