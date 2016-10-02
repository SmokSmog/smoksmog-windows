# # AppVeyor
# $projectPath = "C:\projects\smoksmog-windows"
# $pythonPath= "C:\Python35"
$Configuration = "AppVeyor"

# #local
$projectPath = "D:\dev\Visual Studio 2015\Projects\smoksmog-windows"
$pythonPath= "C:\Program Files (x86)\Python35-32"
#$Configuration = "Debug"

# Setup Python and get codecov script
$pythonScripts = "$pythonPath\Scripts"
& "$pythonScripts\pip" install codecov > $null

# Visual Studio 2015 Tests
$msTest = "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\MSTest.exe"
$vsTest = "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"

# Search for OpenCover in packages
$openCoverPath = @(gci "$projectPath\packages\OpenCover*")[0]
$openCoverConsole = "$openCoverPath\tools\OpenCover.Console.exe"

Write-Host $openCoverConsole

$testProjects = @(gci "$projectPath\*Tests")
$testResultsPath = "$projectPath\TestResults"
New-Item -ItemType Directory -Force -Path $testResultsPath 2> $null > $null

foreach ($testProject in $testProjects) {
	Write-Host $testProject

	$csprojPath = @(gci "$testProject\*.csproj")[0]
	Write-Host $csprojPath

	$file = [xml](gc $csprojPath)
	$assemblyName = ($file.Project.PropertyGroup[0].AssemblyName)
	$outputType = ($file.Project.PropertyGroup[0].OutputType)
	$outputPath = "bin\$Configuration"

	$testContainer = """$testProject\$outputPath\$assemblyName.dll"""
	if($outputType -ne "Library"){
		continue
		$testContainer = """$testProject\$outputPath\$assemblyName.exe"""
	}

	# Manual run MSTest
	#& $msTest /noresults /noisolation /testcontainer:"$testContainer"

	$outputCoverage = """$testResultsPath\$assemblyName.coverage.xml"""

	Write-Host $outputCoverage

	& $openCoverConsole `
		-register:user `
		-output:$outputCoverage `
		"-filter:+[SmokSmog*]* -[*Tests]*" `
		"-excludebyattribute:*.ExcludeFromCodeCoverage*" `
		-hideskipped:All `
		-target:$msTest `
		-targetargs:"/noresults /noisolation /testcontainer:""$testContainer"

	& "$pythonScripts\codecov" -f $outputCoverage -X gcov 2> $null
}