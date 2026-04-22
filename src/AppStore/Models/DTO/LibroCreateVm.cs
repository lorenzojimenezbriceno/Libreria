using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppStore.Models.DTO;

public class LibroCreateVm
{
    public decimal? Precio { get; set; }
    
    [Required(ErrorMessage = "El campo Título es requerido.")]
    public string? Titulo { get; set; }
    
    public string? Descripcion { get; set; }
    
    public DateTime? CreationDate { get; set; }

    [Required(ErrorMessage = "El campo Autor es requerido.")]
    public string? Autor { get; set; }

    // Lista de IDs de categorías seleccionadas
    [Required(ErrorMessage = "Debe seleccionar al menos una categoría.")]
    public List<int>? Categorias { get; set; }

    // Archivo de imagen subido
    public IFormFile? ImageFile { get; set; }

    // Lista de categorías para el dropdown
    public IEnumerable<SelectListItem>? CategoriasList { get; set; }
}
