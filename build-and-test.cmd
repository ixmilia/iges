@echo off

:: build library
set LIBRARY_PROJECT=%~dp0src\IxMilia.Iges\IxMilia.Iges.csproj
dotnet restore "%LIBRARY_PROJECT%"
if errorlevel 1 goto error
dotnet build "%LIBRARY_PROJECT%"
if errorlevel 1 goto error

:: build and run tests
set TEST_PROJECT=%~dp0src\IxMilia.Iges.Test\IxMilia.Iges.Test.csproj
dotnet restore "%TEST_PROJECT%"
if errorlevel 1 goto error
dotnet build "%TEST_PROJECT%"
if errorlevel 1 goto error
dotnet test "%TEST_PROJECT%"
if errorlevel 1 goto error
goto :eof

:error
echo Error building project.
exit /b 1
