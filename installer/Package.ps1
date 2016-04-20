Remove-Item nuget/lib -Force
Remove-Item *.nupkg -Force
New-Item -ItemType directory -Path nuget/lib/net45
Copy-Item ../source/Sumerics/bin/release/* nuget/lib/net45/ -Recurse -Force
Get-ChildItem nuget/lib/net45 -Filter *.vshost.* -Recurse | Remove-Item
Get-ChildItem nuget/lib/net45 -Filter *.pdb -Recurse | Remove-Item
Nuget pack nuget/Sumerics.nuspec
$fn = Get-ChildItem Sumerics*.nupkg
..\source\packages\squirrel.windows.1.3.0\tools\squirrel.exe --releasify $fn
Remove-Item *.nupkg