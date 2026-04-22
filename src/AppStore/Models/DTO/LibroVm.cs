namespace AppStore.Models.DTO;

public class LibroVm
{
    public int Id { get; set; }
    public decimal? Precio { get; set; }
    public string? Titulo { get; set; }
    public string? Descripcion { get; set; }
    public DateTime? CreationDate { get; set; }
    public string? Imagen { get; set; }
    public string? Autor { get; set; }
    public string? CategoriasNames { get; set; }
}
