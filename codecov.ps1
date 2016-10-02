#AppVeyor
$projectPath = "C:\projects\smoksmog-windows"
SET PATH=C:\\Python34;C:\\Python34\\Scripts;%PATH%
pip install codecov
$Configuration = "Release"
$Platform = "AnyCPU"

# #local
# $projectPath = "D:\dev\Visual Studio 2015\Projects\smoksmog-windows"
# $Configuration = "Debug"
# $Platform = "AnyCPU"
#

# Visual Studio 2015 MSTest
$msTest = "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\MSTest.exe"
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
	$outputPath = ($file.Project.PropertyGroup | ? Condition -Like "*$Configuration|$Platform*" | select OutputPath).OutputPath
	
	Write-Host $assemblyName
	Write-Host $outputPath
	
	$testContainer = """$testProject\$outputPath\$assemblyName.dll"""
	
	Write-Host $testContainer
	
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

	codecov -f $outputCoverage
}


