# Libreria
Ejercicio de .NET University

mkdir Libreria
dotnet new globaljson
dotnet new sln --name AppStoreSolution
mkdir src
dotnet new mvc -o src/AppStore
dotnet run --project src/AppStore
cd src
cd AppStore
dotnet add package Microsoft.EntityFrameworkCore --version "9.0.4"
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version "9.0.4"
dotnet add package Microsoft.EntityFrameworkCore.Tools --version "9.0.4"
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version "9.0.4"
