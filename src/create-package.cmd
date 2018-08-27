@echo off

set PROJECT=%~dp0IxMilia.Iges\IxMilia.Iges.csproj
dotnet restore %PROJECT%
if errorlevel 1 exit /b 1
dotnet pack --configuration Release %PROJECT% /p:OfficialBuild=true
