$dependenciesPath = "$($env:USERPROFILE)\dependencies"
New-Item -ItemType Directory -Force -Path $dependenciesPath 2> $null > $null

# Install MultilingualAppToolkit.msi
$msiPath = "$dependenciesPath\MultilingualAppToolkit.msi"
If ((Test-Path $msiPath) –eq $false){
	(New-Object Net.WebClient).DownloadFile('https://visualstudiogallery.msdn.microsoft.com/6dab9154-a7e1-46e4-bbfa-18b5e81df520/file/143697/18/MultilingualAppToolkit.msi', $msiPath)
}
echo "Start installing MultilingualAppToolkit"
cmd /c start /wait msiexec /i $msiPath /q
echo "End installing MultilingualAppToolkit"

# Install Bing Maps SDK for Windows 8.1 Store apps
$vsixPath = "$dependenciesPath\Bing.Maps.vsix"
If ((Test-Path $vsixPath) –eq $false){
	(New-Object Net.WebClient).DownloadFile('https://visualstudiogallery.msdn.microsoft.com/224eb93a-ebc4-46ba-9be7-90ee777ad9e1/file/113087/6/Bing.Maps.vsix', $vsixPath)
}

$vsixCommand = "$dependenciesPath\install-vsix.cmd"
If ((Test-Path $vsixCommand) –eq $false){
	"`"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\VSIXInstaller.exe`" /q /a `"$vsixPath`"" | out-file "$dependenciesPath\install-vsix.cmd" -Encoding ASCII
}

echo "Start installing Bing Maps SDK for Windows 8.1 Store apps"
& $vsixCommand
echo "End installing Bing Maps SDK for Windows 8.1 Store apps"
