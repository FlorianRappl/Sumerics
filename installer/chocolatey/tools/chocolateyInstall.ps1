$packageName = 'Sumerics'
$installerType = 'exe' # Squirell Installer / Updater
$url32 = 'https://github.com/FlorianRappl/Sumerics/releases/download/v2.0.0/Sumerics.exe'
$silentArgs = "" # The installation is silent by default
 
Install-ChocolateyPackage "$packageName" "$installerType" "$silentArgs" "$url32"