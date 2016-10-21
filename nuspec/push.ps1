.\nuget push *.symbols.nupkg -Source https://nuget.smbsrc.net/
del *.symbols.nupkg
.\nuget push *.nupkg -Source https://www.nuget.org
