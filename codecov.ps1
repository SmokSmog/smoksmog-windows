# # AppVeyor
$projectPath = "C:\projects\smoksmog-windows"
$pythonPath= "C:\Python35"

# #local
#$projectPath = "D:\dev\Visual Studio 2015\Projects\smoksmog-windows"
#$pythonPath= "C:\Program Files (x86)\Python35-32"

# Build setup
$Configuration = "Release"

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

$testProjects = @(gci "$projectPath\Tests\*Tests")
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
		#$testContainer = """$testProject\$outputPath\$assemblyName.exe"""
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

#$AppPackage = "$projectPath\SmokSmog.Core.WinRT.Tests\AppPackages\SmokSmog.Core.WinRT.Tests_1.0.0.0_x86_AppVeyor_Test"
#Certutil -addstore root "$AppPackage\SmokSmog.Core.WinRT.Tests_1.0.0.0_x86_AppVeyor.cer"
##Manually run vsTest
## & $vsTest /Platform:x86 /InIsolation "$AppPackage\SmokSmog.Core.WinRT.Tests_1.0.0.0_x86_AppVeyor.appx"

##-targetargs:"/Logger:trx /inIsolation /Platform:x64 unittest.dll" /resultsfile:"unittest.trx"
##-filter:"+[*]*"

#& $openCoverConsole `
#	-register:user `
#	-output:"""$AppPackage\coverage.xml""" `
#	"-filter:+[SmokSmog*]* -[*Tests]* -[*TestFramework*]* -[*]Microsoft.ApplicationInsights.Extensibility.Implementation.External*" `
#	"-excludebyattribute:*.ExcludeFromCodeCoverage*" `
#	-hideskipped:All `
#	-target:$vsTest `
#	-targetargs:"/Logger:trx /Platform:x86 /InIsolation """"$AppPackage\SmokSmog.Core.WinRT.Tests_1.0.0.0_x86_AppVeyor.appx""" `
#	-mergebyhash
#	 #/resultsfile:"""$AppPackage/unittest.trx""" `

## & "$pythonScripts\codecov" -f """$AppPackage\coverage.xml""" -X gcov 2> $null

##..\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -register:user
##  "-target:C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"

##  "-targetargs:..\a\Release\Src\DependencyCollector\Net40.Tests\Microsoft.ApplicationInsights.DependencyCollector.Net40.Tests.dll  [CUT: all assemblies]
##  ..\a\Release\Src\WindowsServer\WindowsServer.Nuget.Tests\WindowsServer.Nuget.Tests.dll /TestCaseFilter:TestCategory!=Required_4_5_1 /logger:trx"
##  "-filter:+[Microsoft.AI*]* -[*Tests]* -[*TestFramework*]* -[*]Microsoft.ApplicationInsights.Extensibility.Implementation.External*" -hideskipped:All -output:.\coverage.xml