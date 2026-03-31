using Microsoft.AspNetCore.Identity;

namespace AppStore.Models.Context;

public class LoadDatabase
{
    public static async Task InsertarData(
        DatabaseContext context,
        UserManager<ApplicationUser> usuarioManager,
        RoleManager<IdentityRole> roleManager)
    {
        // Agregar los roles si no existen
        if (!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("User"));
        }

        // Agregar los usuarios si no existen
        if (!usuarioManager.Users.Any())
        {
            // Agregar un usuario administrador
            var adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@example.com"
            };
            await usuarioManager.CreateAsync(adminUser, "Admin123!");
            await usuarioManager.AddToRoleAsync(adminUser, "Admin");

            // Agregar un usuario regular
            var user = new ApplicationUser
            {
                UserName = "user",
                Email = "user@example.com"
            };
            await usuarioManager.CreateAsync(user, "User123!");
            await usuarioManager.AddToRoleAsync(user, "User");
        }

        // Agregar categorias
        if (!context.Categorias.Any())
        {
            context.Categorias.AddRange(
                new Categoria { Nombre = "Drama" },
                new Categoria { Nombre = "Comedia" },
                new Categoria { Nombre = "Accion" },
                new Categoria { Nombre = "Terror" },
                new Categoria { Nombre = "Documentales" },
                new Categoria { Nombre = "Aventura" },
                new Categoria { Nombre = "Juegos" },
                new Categoria { Nombre = "Productividad" },
                new Categoria { Nombre = "Educación" },
                new Categoria { Nombre = "Misterio" }
            );
        }

        // Agregar libros
        if (!context.Libros.Any())
        {
            context.Libros.AddRange(
                new Libro
                {
                    Titulo = "El Gran Gatsby",
                    Autor = "F. Scott Fitzgerald",
                    Descripcion = "Una novela clásica sobre el sueño americano.",
                    Precio = 9.99m,
                    CreationDate = "10/04/1925",
                    Imagen = "https://images-na.ssl-images-amazon.com/images/I/81af+MCATTL.jpg",
                    CategoriaId = context.Categorias.First(c => c.Nombre == "Drama").Id
                },
                new Libro
                {
                    Titulo = "Cien Años de Soledad",
                    Autor = "Gabriel García Márquez",
                    Descripcion = "Una obra maestra del realismo mágico.",
                    Precio = 12.99m,
                    CreationDate = "30/05/1967",
                    Imagen = "https://images-na.ssl-images-amazon.com/images/I/81t2CVWEsUL.jpg",
                    CategoriaId = context.Categorias.First(c => c.Nombre == "Drama").Id
                },
                new Libro
                {
                    Titulo = "La Sombra del Viento",
                    Autor = "Carlos Ruiz Zafón",
                    Descripcion = "Un misterio literario ambientado en Barcelona.",
                    Precio = 10.99m,
                    CreationDate = "01/05/2001",
                    Imagen = "https://images-na.ssl-images-amazon.com/images/I/81a4kCNuH+L.jpg",
                    CategoriaId = context.Categorias.First(c => c.Nombre == "Misterio").Id
                }
            );
        }

        // Agregar LibroCategorias
        if (!context.LibroCategorias.Any())
        {
            context.LibroCategorias.AddRange(
                new LibroCategoria { CategoriaId = 1, LibroId = 1 },
                new LibroCategoria { CategoriaId = 1, LibroId = 2 },
                new LibroCategoria { CategoriaId = 10, LibroId = 3 }
            );
        }

        await context.SaveChangesAsync();
    }
}