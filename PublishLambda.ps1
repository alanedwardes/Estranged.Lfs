$ErrorActionPreference = "Stop"

$OutputFolder = "hosting/Estranged.Lfs.Hosting.Lambda/bin/Release/netcoreapp3.1/linux-x64/publish"
$OutputFile = "Estranged.Lfs.Hosting.Lambda.zip"
$InputProject = "hosting/Estranged.Lfs.Hosting.Lambda/Estranged.Lfs.Hosting.Lambda.csproj"

Remove-Item -LiteralPath $OutputFile -ErrorAction Ignore
Remove-Item -LiteralPath $OutputFolder -Force -Recurse -ErrorAction Ignore
dotnet publish $InputProject --runtime linux-x64 --configuration Release
Compress-Archive -Path $OutputFolder/* -DestinationPath $OutputFile