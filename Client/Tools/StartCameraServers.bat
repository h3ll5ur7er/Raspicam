@echo off

(arp -a | grep " b8-27-eb-\(85-df-31\|39-e5-86\)" | sed -n "s/\s*\([0-9]\{1,3\}\.[0-9]\{1,3\}\.[0-9]\{1,3\}\.[0-9]\{1,3\}\).*/\1/1p") >piAdresses

for /F %%i in (piAdresses) do (start ansicon plink -ssh pi@%%i -pw raspberry sudo mono Raspicam/Server\ Rasbian/RaspicamServer.exe)
rem for /F %%i in (piAdresses) do (start ansicon plink -ssh pi@%%i -pw raspberry sudo reboot)
