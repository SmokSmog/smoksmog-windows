$projectPath = "."

if($env:APPVEYOR)
{
	$projectPath = $env:APPVEYOR_BUILD_FOLDER
}

Import-PfxCertificate -FilePath "$projectPath\App\SmokSmog.WindowsUniversal\SmokSmog.App.UWP_TemporaryKey.pfx" -CertStoreLocation cert:\CurrentUser\My > $null 2> $null
Import-PfxCertificate -FilePath "$projectPath\App\SmokSmog.Windows\SmokSmog.App.win81_TemporaryKey.pfx" -CertStoreLocation cert:\CurrentUser\My > $null 2> $null
Import-PfxCertificate -FilePath "$projectPath\Tests\SmokSmog.Core.WinRT.Tests\SmokSmog.Core.WinRT.Tests_TemporaryKey.pfx" -CertStoreLocation cert:\CurrentUser\My > $null 2> $null