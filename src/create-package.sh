#!/bin/sh -e

PROJECT=./IxMilia.Iges/IxMilia.Iges.csproj
dotnet restore $PROJECT
dotnet pack --include-symbols --include-source --configuration Release $PROJECT

