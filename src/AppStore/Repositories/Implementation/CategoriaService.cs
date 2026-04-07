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

    public bool Add(Categoria categoria)
    {
        try
        {
            // Salvar el libro en la base de datos
            _context.Categorias!.Add(categoria);
            _context.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool Delete(int id)
    {
        try
        {
            // Eliminar el libro en la base de datos
            var categoria = GetById(id);
            if (categoria == null)
            {
                return false;
            }

            _context.Categorias!.Remove(categoria);
            _context.SaveChanges();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool Update(Categoria categoria)
    {
        try
        {
            _context.Categorias!.Update(categoria);
            _context.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    
    public Categoria? GetById(int id)
    {
        return _context.Categorias!.Find(id);
    }
}
