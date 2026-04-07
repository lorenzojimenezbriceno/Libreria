using AppStore.Models.Domain;

namespace AppStore.Models.DTO;

public class CategoriaListVm
{
    public IQueryable<Categoria>? CategoriasList { get; set; }
}