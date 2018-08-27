#!/bin/sh -e

_SCRIPT_DIR="$( cd -P -- "$(dirname -- "$(command -v -- "$0")")" && pwd -P )"
PROJECT=$_SCRIPT_DIR/IxMilia.Iges/IxMilia.Iges.csproj
dotnet restore $PROJECT
dotnet pack --configuration Release $PROJECT /p:OfficialBuild=true
