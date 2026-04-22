# Libreria

Ejercicio de .NET University, curso Foundational C# usando .NET y MVC

# Falta / arreglar

1. La paginacion de libros.
2. La busqueda de libros.

# Creación del proyecto

mkdir Libreria
dotnet new globaljson
dotnet new sln --name AppStoreSolution
mkdir src
dotnet new mvc -o src/AppStore
dotnet sln add .\src\AppStore\AppStore.csproj
dotnet run --project src/AppStore

cd src
cd AppStore
dotnet add package Microsoft.EntityFrameworkCore --version "9.0.14"
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version "9.0.14"
dotnet add package Microsoft.EntityFrameworkCore.Tools --version "9.0.14"
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version "9.0.14"

# Migración de la base de datos

dotnet tool install --global dotnet-ef --version 9.0.14
dotnet add package Microsoft.EntityFrameworkCore.Design --version "9.0.14"
dotnet ef migrations add MigracionInicial
dotnet ef database update 

# Actualizar paquetes

dotnet remove package Microsoft.EntityFrameworkCore
dotnet remove package Microsoft.EntityFrameworkCore.Sqlite
dotnet remove package Microsoft.EntityFrameworkCore.Tools
dotnet remove package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet remove package Microsoft.EntityFrameworkCore.Design
