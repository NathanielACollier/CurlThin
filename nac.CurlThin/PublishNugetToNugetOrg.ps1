$buildConfigurationName = "Debug"


$scriptParentFolderPath = [System.IO.Path]::GetDirectoryName( $MyInvocation.InvocationName )

$buildPath = [system.io.path]::Combine( $scriptParentFolderPath, "bin", $buildConfigurationName)
$nugetPackages = Get-ChildItem $buildPath -Filter *.nupkg
$package = $nugetPackages | Sort-Object -Descending -Property CreationTIme | Select-Object -First 1

$settings = (Get-Content '~/settings.json' | Out-String | ConvertFrom-Json)

<#
Example from: https://docs.microsoft.com/en-us/nuget/nuget-org/publish-a-package
dotnet nuget push AppLogger.1.0.0.nupkg --api-key <put key here> --source https://api.nuget.org/v3/index.json
#>

& dotnet @("nuget","push", $package.FullName, "--api-key", $settings.nugetOrgAPIKey, "--source",
         "https://api.nuget.org/v3/index.json" 
 )
 
 