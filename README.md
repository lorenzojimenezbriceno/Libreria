# Libreria

Ejercicio de .NET University, curso Foundational C# usando .NET y MVC

# Creación del proyecto

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

# Migración de la base de datos

dotnet tool install --global dotnet-ef --version 9.0.4
dotnet add package Microsoft.EntityFrameworkCore.Design --version "9.0.4"
dotnet ef migrations add MigracionInicial
dotnet ef database update 
