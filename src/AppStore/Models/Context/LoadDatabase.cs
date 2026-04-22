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
                // 20 libros de diferentes géneros
                new Libro
                {
                    Titulo = "El Gran Gatsby",
                    Autor = "F. Scott Fitzgerald",
                    Descripcion = "Una novela clásica sobre el sueño americano.",
                    Precio = 9.99m,
                    CreationDate = new DateTime(1925, 4, 10),
                    Imagen = "https://picsum.photos/seed/book1/200/300"
                },
                new Libro
                {
                    Titulo = "Cien Años de Soledad",
                    Autor = "Gabriel García Márquez",
                    Descripcion = "Una obra maestra del realismo mágico.",
                    Precio = 12.99m,
                    CreationDate = new DateTime(1967, 5, 30),
                    Imagen = "https://picsum.photos/seed/book2/200/300"
                },
                new Libro
                {
                    Titulo = "La Sombra del Viento",
                    Autor = "Carlos Ruiz Zafón",
                    Descripcion = "Un misterio literario ambientado en Barcelona.",
                    Precio = 10.99m,
                    CreationDate = new DateTime(2001, 5, 1),
                    Imagen = "https://picsum.photos/seed/book3/200/300"
                },
                new Libro
                {
                    Titulo = "El Código Da Vinci",
                    Autor = "Dan Brown",
                    Descripcion = "Thriller de misterio y símbolos religiosos.",
                    Precio = 8.99m,
                    CreationDate = new DateTime(2003, 3, 18),
                    Imagen = "https://picsum.photos/seed/book4/200/300"
                },
                new Libro
                {
                    Titulo = "Juego de Tronos",
                    Autor = "George R.R. Martin",
                    Descripcion = "Primera entrega de la saga épica de fantasía.",
                    Precio = 14.99m,
                    CreationDate = new DateTime(1996, 8, 6),
                    Imagen = "https://picsum.photos/seed/book5/200/300"
                },
                new Libro
                {
                    Titulo = "El Principito",
                    Autor = "Antoine de Saint-Exupéry",
                    Descripcion = "Cuento poético y filosófico para niños y adultos.",
                    Precio = 5.99m,
                    CreationDate = new DateTime(1943, 4, 6),
                    Imagen = "https://picsum.photos/seed/book6/200/300"
                },
                new Libro
                {
                    Titulo = "La Guerra del Mundo",
                    Autor = "H. G. Wells",
                    Descripcion = "Clásico de ciencia ficción y conflicto interestelar.",
                    Precio = 7.49m,
                    CreationDate = new DateTime(1898, 1, 1),
                    Imagen = "https://picsum.photos/seed/book7/200/300"
                },
                new Libro
                {
                    Titulo = "Sapiens: De animales a dioses",
                    Autor = "Yuval Noah Harari",
                    Descripcion = "Historia breve de la humanidad.",
                    Precio = 15.99m,
                    CreationDate = new DateTime(2011, 2, 4),
                    Imagen = "https://picsum.photos/seed/book8/200/300"
                },
                new Libro
                {
                    Titulo = "Los Juegos del Hambre",
                    Autor = "Suzanne Collins",
                    Descripcion = "Distopía donde adolescentes luchan por sobrevivir.",
                    Precio = 9.49m,
                    CreationDate = new DateTime(2008, 9, 14),
                    Imagen = "https://picsum.photos/seed/book9/200/300"
                },
                new Libro
                {
                    Titulo = "El Arte de la Guerra",
                    Autor = "Sun Tzu",
                    Descripcion = "Estrategia militar y empresarial.",
                    Precio = 6.99m,
                    CreationDate = new DateTime(500, 1, 1),
                    Imagen = "https://picsum.photos/seed/book10/200/300"
                },
                new Libro
                {
                    Titulo = "Harry Potter y la piedra filosofal",
                    Autor = "J.K. Rowling",
                    Descripcion = "Primer libro de la saga mágica.",
                    Precio = 11.99m,
                    CreationDate = new DateTime(1997, 6, 26),
                    Imagen = "https://picsum.photos/seed/book11/200/300"
                },
                new Libro
                {
                    Titulo = "El Diario de Ana Frank",
                    Autor = "Anne Frank",
                    Descripcion = "Testimonio emotivo de la vida oculta durante la Segunda Guerra Mundial.",
                    Precio = 8.49m,
                    CreationDate = new DateTime(1947, 6, 25),
                    Imagen = "https://picsum.photos/seed/book12/200/300"
                },
                new Libro
                {
                    Titulo = "Ready Player One",
                    Autor = "Ernest Cline",
                    Descripcion = "Aventura de realidad virtual y cultura pop.",
                    Precio = 10.49m,
                    CreationDate = new DateTime(2011, 8, 16),
                    Imagen = "https://picsum.photos/seed/book13/200/300"
                },
                new Libro
                {
                    Titulo = "El Alquimista",
                    Autor = "Paulo Coelho",
                    Descripcion = "Fábula sobre seguir los sueños.",
                    Precio = 9.99m,
                    CreationDate = new DateTime(1988, 4, 1),
                    Imagen = "https://picsum.photos/seed/book14/200/300"
                },
                new Libro
                {
                    Titulo = "El hombre más rico de Babilonia",
                    Autor = "George S. Clason",
                    Descripcion = "Lecciones financieras atemporales.",
                    Precio = 7.99m,
                    CreationDate = new DateTime(1926, 1, 1),
                    Imagen = "https://picsum.photos/seed/book15/200/300"
                },
                new Libro
                {
                    Titulo = "Matar a un ruiseñor",
                    Autor = "Harper Lee",
                    Descripcion = "Historia sobre la injusticia racial en EE.UU.",
                    Precio = 12.49m,
                    CreationDate = new DateTime(1960, 7, 11),
                    Imagen = "https://picsum.photos/seed/book16/200/300"
                },
                new Libro
                {
                    Titulo = "El Poder del Hábito",
                    Autor = "Charles Duhigg",
                    Descripcion = "Cómo los hábitos influyen en nuestras vidas.",
                    Precio = 13.99m,
                    CreationDate = new DateTime(2012, 2, 28),
                    Imagen = "https://picsum.photos/seed/book17/200/300"
                },
                new Libro
                {
                    Titulo = "Clean Code",
                    Autor = "Robert C. Martin",
                    Descripcion = "Principios para escribir código limpio.",
                    Precio = 25.00m,
                    CreationDate = new DateTime(2008, 8, 1),
                    Imagen = "https://picsum.photos/seed/book18/200/300"
                },
                new Libro
                {
                    Titulo = "El Mito del Emprendedor",
                    Autor = "Michael E. Gerber",
                    Descripcion = "Por qué la mayoría de las pequeñas empresas no funcionan.",
                    Precio = 15.50m,
                    CreationDate = new DateTime(1986, 1, 1),
                    Imagen = "https://picsum.photos/seed/book19/200/300"
                }
            );
            await context.SaveChangesAsync();

            // Relacionar libros con categorías (al menos una categoría por libro)
            await context.LibroCategorias!.AddRangeAsync(
                // Asignaciones manuales (LibroId empieza en 1 y sigue el orden anterior)
                new LibroCategoria { LibroId = 1, CategoriaId = 1 },   // El Gran Gatsby -> Drama
                new LibroCategoria { LibroId = 2, CategoriaId = 2 },   // Cien Años de Soledad -> Comedia
                new LibroCategoria { LibroId = 3, CategoriaId = 10 }, // La Sombra del Viento -> Misterio
                new LibroCategoria { LibroId = 4, CategoriaId = 4 },   // El Código Da Vinci -> Terror
                new LibroCategoria { LibroId = 5, CategoriaId = 3 },   // Juego de Tronos -> Accion
                new LibroCategoria { LibroId = 6, CategoriaId = 7 },   // El Principito -> Juegos
                new LibroCategoria { LibroId = 7, CategoriaId = 5 },   // La Guerra del Mundo -> Documentales
                new LibroCategoria { LibroId = 8, CategoriaId = 9 },   // Sapiens -> Educación
                new LibroCategoria { LibroId = 9, CategoriaId = 8 },   // Los Juegos del Hambre -> Productividad
                new LibroCategoria { LibroId = 10, CategoriaId = 6 },  // El Arte de la Guerra -> Aventura
                new LibroCategoria { LibroId = 11, CategoriaId = 2 },  // Harry Potter -> Comedia
                new LibroCategoria { LibroId = 12, CategoriaId = 1 },  // Diario de Ana Frank -> Drama
                new LibroCategoria { LibroId = 13, CategoriaId = 5 },  // Ready Player One -> Documentales
                new LibroCategoria { LibroId = 14, CategoriaId = 6 },  // El Alquimista -> Aventura
                new LibroCategoria { LibroId = 15, CategoriaId = 8 },  // Hombre más rico de Babilonia -> Productividad
                new LibroCategoria { LibroId = 16, CategoriaId = 1 },  // Matar a un ruiseñor -> Drama
                new LibroCategoria { LibroId = 17, CategoriaId = 4 },  // El Poder del Hábito -> Terror
                new LibroCategoria { LibroId = 18, CategoriaId = 8 },  // Clean Code -> Productividad
                new LibroCategoria { LibroId = 19, CategoriaId = 8 }   // El Mito del Emprendedor -> Productividad
            );
            await context.SaveChangesAsync();
        }
    }
}