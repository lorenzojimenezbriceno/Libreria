using System.ComponentModel.DataAnnotations;

namespace AppStore.Models.Domain;

public class Libro
{
    [Key]
    [Required]
    public int Id { get; set; }
    public decimal? Precio { get; set; }
    public string? Titulo { get; set; }
    public string? Descripcion { get; set; }
    public string? CreationDate { get; set; }
    public string? Imagen { get; set; }
    [Required]
    public string? Autor { get; set; }

    // Referencias
    public virtual ICollection<Categoria>? CategoriaRelationList { get; set; }
    public virtual ICollection<LibroCategoria>? LibroCategoriaRelationList { get; set; }
}
