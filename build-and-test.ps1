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

try {
    # build
    $solution = (Get-Item "$PSScriptRoot/IxMilia.*.sln")[0]
    dotnet restore $solution || Fail "Failed to restore solution"
    dotnet build $solution --configuration $configuration || Fail "Failed to build solution"

    # test
    if (-Not $noTest) {
        dotnet test --no-restore --no-build --configuration $configuration || Fail "Error running tests."
    }

    # create package
    dotnet pack --no-restore --no-build --configuration $configuration $solution || Fail "Error creating package."
}
catch {
    Write-Host $_
    Write-Host $_.Exception
    Write-Host $_.ScriptStackTrace
    exit 1
}
