$proj = "../src/ape.umbraco/ape.umbraco.csproj"
$packageDir = "../src/packages"
$buildDir = "../../Build/bin/";
$netVersions = "4.5.1","4.6.1";

./nuget.exe restore $proj -outputdirectory $packageDir

$msbuild = ${env:ProgramFiles(x86)}+"\MSBuild\14.0\Bin\msbuild.exe"

foreach ($item in $netVersions) {
& $msbuild $proj /p:configuration="Release" /p:TargetFrameworkVersion=v$item /p:OutDir=$buildDir$item /t:rebuild
}

# Build nuget package:
$package = "../Nuget/APE.Umbraco.3.2.0-beta004.nuspec";

./nuget pack $package -verbosity detailed