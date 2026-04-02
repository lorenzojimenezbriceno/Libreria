using AppStore.Repositories.Abstract;
using AppStore.Models.Domain;
using AppStore.Models.Context;

namespace AppStore.Repositories.Implementation;

public class CategoriaService : ICategoriaService
{
    private readonly DatabaseContext _context;

    public CategoriaService(DatabaseContext context)
    {
        _context = context;
    }

    public IQueryable<Categoria> List()
    {
        return _context.Categorias!.AsQueryable();
    }
}
