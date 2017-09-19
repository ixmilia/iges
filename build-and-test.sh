#!/bin/sh -e

_SCRIPT_DIR="$( cd -P -- "$(dirname -- "$(command -v -- "$0")")" && pwd -P )"

# build library
LIBRARY_PROJECT=$_SCRIPT_DIR/src/IxMilia.Iges/IxMilia.Iges.csproj
dotnet restore "$LIBRARY_PROJECT"
dotnet build "$LIBRARY_PROJECT"

# build and run tests
TEST_PROJECT=$_SCRIPT_DIR/src/IxMilia.Iges.Test/IxMilia.Iges.Test.csproj
dotnet restore "$TEST_PROJECT"
dotnet build "$TEST_PROJECT"
dotnet test "$TEST_PROJECT"
