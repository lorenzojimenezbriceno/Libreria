using Microsoft.AspNetCore.Identity;
using AppStore.Models.Domain;

namespace AppStore.Models.Context;

public class LoadDatabase
{
    public static async Task InsertarData(
        DatabaseContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        // Agregar los roles si no existen
        if (!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("User"));
        }

        // Agregar los usuarios si no existen
        if (!userManager.Users.Any())
        {
            // Agregar un usuario administrador
            var adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@example.com",
                Nombre = "Administrador",
            };
            await userManager.CreateAsync(adminUser, "Admin123!");
            await userManager.AddToRoleAsync(adminUser, "Admin");

            // Agregar un usuario regular
            var user = new ApplicationUser
            {
                UserName = "user",
                Email = "user@example.com",
                Nombre = "Lorenzo",
            };
            await userManager.CreateAsync(user, "User123!");
            await userManager.AddToRoleAsync(user, "User");
            await context.SaveChangesAsync();
        }

        // Agregar categorias
        if (!context.Categorias!.Any())
        {
            await context.Categorias!.AddRangeAsync(
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
            await context.SaveChangesAsync();
        }

        // Agregar libros
        if (!context.Libros!.Any())
        {
            await context.Libros!.AddRangeAsync(
                new Libro
                {
                    Titulo = "El Gran Gatsby",
                    Autor = "F. Scott Fitzgerald",
                    Descripcion = "Una novela clásica sobre el sueño americano.",
                    Precio = 9.99m,
                    CreationDate = new DateTime(1925, 4, 10),
                    Imagen = "https://images-na.ssl-images-amazon.com/images/I/81af+MCATTL.jpg"
                },
                new Libro
                {
                    Titulo = "Cien Años de Soledad",
                    Autor = "Gabriel García Márquez",
                    Descripcion = "Una obra maestra del realismo mágico.",
                    Precio = 12.99m,
                    CreationDate = new DateTime(1967, 5, 30),
                    Imagen = "https://images-na.ssl-images-amazon.com/images/I/81t2CVWEsUL.jpg"
                },
                new Libro
                {
                    Titulo = "La Sombra del Viento",
                    Autor = "Carlos Ruiz Zafón",
                    Descripcion = "Un misterio literario ambientado en Barcelona.",
                    Precio = 10.99m,
                    CreationDate = new DateTime(2001, 5, 1),
                    Imagen = "https://images-na.ssl-images-amazon.com/images/I/81a4kCNuH+L.jpg"
                }
            );
            await context.SaveChangesAsync();
        }

        // Agregar LibroCategorias
        if (!context.LibroCategorias!.Any())
        {
            await context.LibroCategorias!.AddRangeAsync(
                new LibroCategoria { CategoriaId = 1, LibroId = 1 },
                new LibroCategoria { CategoriaId = 2, LibroId = 2 },
                new LibroCategoria { CategoriaId = 10, LibroId = 3 }
            );
            await context.SaveChangesAsync();
        }
    }
}