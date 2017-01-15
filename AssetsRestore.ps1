$win81 = [pscustomobject]@{
	Name = "win81"
	Folder = "./App/SmokSmog.Windows/"
	Scales = 180,140,100,80
	Target = 16,24,32,48,256
	Assets = 7,9,10,11,2,6,1,4
}

$wp81 = [pscustomobject]@{
	Name = "wp81"
	Folder = "./App/SmokSmog.WindowsPhone/"
	Scales = 240,140,100
	Target = 0
	Assets = 8,9,10,3,6,1,5
}

$uwp = [pscustomobject]@{
	Name = "uwp"
	Folder = "./App/SmokSmog.WindowsUniversal/"
	Scales = 400,200,150,125,100
	Target = 16,24,32,48,256
	Assets = 8,9,10,11,3,6,1,4
}

$projects = $win81,$wp81,$uwp

$filesNames =@{
 1 = "Assets\Badge\BadgeLogo";
 2 = "Assets\Icon\Square30x30Logo";
 3 = "Assets\Icon\Square44x44Logo";
 4 = "Assets\SplashScreen\SplashScreenHorizontal";
 5 = "Assets\SplashScreen\SplashScreenVertical";
 6 = "Assets\Store\StoreLogo";
 7 = "Assets\Tiles\Small\Square70x70Logo";
 8 = "Assets\Tiles\Small\Square71x71Logo";
 9 = "Assets\Tiles\Medium\Square150x150Logo";
10 = "Assets\Tiles\Wide\Wide310x150Logo";
11 = "Assets\Tiles\Large\Square310x310Logo"}

$folders = "Badge", "Icon", "SplashScreen", "Store", "Tiles"
$tiles = "Small","Medium","Wide","Large"
$sharedPath = "./App/SmokSmog.Shared/"

foreach ($project in $projects) {
	$path = "$($project.Folder)"

	foreach ($folder in $folders) {
		New-Item -ItemType Directory -Force -Path "$path/Assets/$folder"
	}

	foreach ($folder in $tiles) {
		New-Item -ItemType Directory -Force -Path "$path/Assets/Tiles/$folder"
	}

	foreach ($asset in $project.Assets){
		$assetName = $filesNames.Item($asset)
		foreach ($scale in $project.Scales){
			$assetPath = "$assetName.scale-$scale.png"

			If (Test-Path "$sharedPath/$assetPath"){
				# // File exists
				Copy-Item "$sharedPath/$assetPath" "$path/$assetPath" -force
			}
		}

		foreach ($target in $project.Target){
			$assetTargetSrcPath = "$assetName.targetsize-$target.png"
			$assetTargetAltPath = "$assetName.targetsize-$($target)_altform-unplated.png"
			
			#Square44x44Logo.targetsize-256_altform-unplated

			If (Test-Path "$sharedPath/$assetTargetSrcPath"){
				# // File exists
				Copy-Item "$sharedPath/$assetTargetSrcPath" "$path/$assetTargetSrcPath" -force
			}
			If (Test-Path "$sharedPath/$assetTargetAltPath"){
				# // File exists
				Copy-Item "$sharedPath/$assetTargetAltPath" "$path/$assetTargetAltPath" -force
			}
		}
	}
}