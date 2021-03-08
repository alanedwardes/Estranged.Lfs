$ErrorActionPreference = "Stop"

Remove-Item src/*/bin/Release/*.nupkg

Foreach ($TestProject in Get-ChildItem tests/*/*.csproj)
{
    dotnet test $TestProject

    if ($LastExitCode -ne 0)
    {
        Read-Host "Tests failed"
        return;
    }
}

Foreach ($SourceProject in Get-ChildItem src/*/*.csproj)
{
    dotnet pack --configuration Release $SourceProject

    if ($LastExitCode -ne 0)
    {
        Read-Host "Pack failed"
        return;
    }
}

$ApiKeySecure = Read-Host "Enter NuGet API key" -AsSecureString

$ApiKey = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto([System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($ApiKeySecure))

Foreach ($Package in Get-ChildItem src/*/bin/Release/*.nupkg)
{
    dotnet nuget push $Package.FullName --api-key $ApiKey --source https://api.nuget.org/v3/index.json --skip-duplicate
}

Read-Host "Package(s) pushed"
