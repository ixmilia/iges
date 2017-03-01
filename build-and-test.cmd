set TEST_PROJECT=.\src\IxMilia.Iges.Test\IxMilia.Iges.Test.csproj
dotnet restore %TEST_PROJECT%
dotnet test %TEST_PROJECT%
