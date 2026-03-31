using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppStore.Models.Domain;

namespace AppStore.Models.Context;

public class DatabaseContext : IdentityDbContext<ApplicationUser>
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Definición de la entidad libro
        modelBuilder.Entity<Libro>()
            .HasMany(
                // un libro tiene muchas categorias
                x => x.CategoriaRelationList) 
            .WithMany(
                // una categoria tiene muchos libros
                y => y.LibroRelationList) 
            .UsingEntity<LibroCategoria>(
                // entidad de relación intermedia muchos a muchos de libros y categorias
                v => v
                    .HasOne(
                        // un libro categoria tiene una categoria
                        a => a.Categoria) 
                    .WithMany(
                        // una categoria tiene muchos libro categorias
                        b => b.LibroCategoriaRelationList) 
                    .HasForeignKey(c => c.CategoriaId),
                w => w
                    .HasOne(d => d.Libro)
                    .WithMany(e => e.LibroCategoriaRelationList)
                    .HasForeignKey(f => f.LibroId),
                z =>
                {
                    // clave primaria compuesta por libro id y categoria id
                    z.HasKey(g => new { g.LibroId, g.CategoriaId });
                }
            );
    }

    public DbSet<Libro> Libros { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<LibroCategoria> LibroCategorias { get; set; }
}