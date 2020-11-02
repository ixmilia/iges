#!/usr/bin/pwsh

[CmdletBinding(PositionalBinding=$false)]
param (
    [string]$configuration = "Debug",
    [switch]$noTest
)

Set-StrictMode -version 2.0
$ErrorActionPreference = "Stop"

function Fail([string]$message) {
    throw $message
}

function Single([string]$pattern) {
    $items = @(Get-Item $pattern)
    if ($items.Length -ne 1) {
        $itemsList = $items -Join "`n"
        Fail "Expected single item, found`n$itemsList`n"
    }

    return $items[0]
}

try {
    # build
    $solution = Single "$PSScriptRoot/*.sln"
    dotnet restore $solution || Fail "Failed to restore solution"
    dotnet build $solution --configuration $configuration || Fail "Failed to build solution"

    # test
    if (-Not $noTest) {
        dotnet test --no-restore --no-build --configuration $configuration || Fail "Error running tests."
    }

    # create package
    dotnet pack --no-restore --no-build --configuration $configuration $solution || Fail "Error creating package."
    $package = Single "$PSScriptRoot/artifacts/packages/$configuration/*.nupkg"
    Write-Host "Package generated at '$package'"
}
catch {
    Write-Host $_
    Write-Host $_.Exception
    Write-Host $_.ScriptStackTrace
    exit 1
}
