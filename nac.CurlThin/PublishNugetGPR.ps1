$buildConfigurationName = "Debug"


$scriptParentFolderPath = [System.IO.Path]::GetDirectoryName( $MyInvocation.InvocationName )

$buildPath = [system.io.path]::Combine( $scriptParentFolderPath, "bin", $buildConfigurationName)
$nugetPackages = Get-ChildItem $buildPath -Filter *.nupkg
$package = $nugetPackages | Sort-Object -Descending -Property CreationTIme | Select-Object -First 1

$settings = (Get-Content '~/settings.json' | Out-String | ConvertFrom-Json)

& gpr @("push", "--api-key", $settings.githubToken, $package.FullName)