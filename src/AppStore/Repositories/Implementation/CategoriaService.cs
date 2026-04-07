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
            var categoria = GetById(id);
            if (categoria == null)
            {
                return false;
            }
        
            // Eliminar las categorias relacionadas con los libros
            var libroCategorias = _context.LibroCategorias!.Where(lc => lc.CategoriaId == categoria.Id).ToList();
            _context.LibroCategorias!.RemoveRange(libroCategorias!);

            // Eliminar la categoria en la base de datos
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
