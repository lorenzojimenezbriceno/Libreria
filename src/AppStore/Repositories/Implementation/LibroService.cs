using AppStore.Models.Domain;
using AppStore.Models.Context;
using AppStore.Models.DTO;

namespace AppStore.Repositories.Abstract;

public class LibroService : ILibroService
{
    private readonly DatabaseContext _context;

    public LibroService(DatabaseContext context)
    {
        _context = context;
    }

    public bool Add(Libro libro, List<int> categorias)
    {
        try
        {
            // Salvar el libro en la base de datos
            _context.Libros!.Add(libro);
            _context.SaveChanges();

            // Guardar las categorías asociadas al libro
            if (categorias != null)
            {
                foreach (var categoria in categorias)
                {
                    var libroCategoria = new LibroCategoria
                    {
                        LibroId = libro.Id,
                        CategoriaId = categoria
                    };
                    _context.LibroCategorias!.Add(libroCategoria);
                }
                _context.SaveChanges();

                return true;
            }

            return false;
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
            var libro = GetById(id);
            if (libro == null)
            {
                return false;
            }

            // Buscar todas las categorías asociadas al libro
            var libroCategorias = _context.LibroCategorias!.Where(lc => lc.LibroId == id).ToList();
            _context.LibroCategorias!.RemoveRange(libroCategorias);
            _context.Libros!.Remove(libro);
            _context.SaveChanges();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool Update(Libro libro, List<int> categorias)
    {
        try
        {
            // Buscar todas las categorías asociadas al libro y eliminelas
            var libroCategorias = _context.LibroCategorias!.Where(lc => lc.LibroId == libro.Id);
            _context.LibroCategorias!.RemoveRange(libroCategorias);
            _context.SaveChanges();

            // Guardar las categorías asociadas al libro
            if (categorias != null)
            {
                foreach (var categoria in categorias)
                {
                    var libroCategoria = new LibroCategoria
                    {
                        LibroId = libro.Id,
                        CategoriaId = categoria
                    };
                    _context.LibroCategorias!.Add(libroCategoria);
                }
                _context.SaveChanges();
            }

            // Actualizar el libro en la base de datos
            _context.Libros!.Update(libro);
            _context.SaveChanges();
            
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    
    public Libro? GetById(int id)
    {
        return _context.Libros!.Find(id);
    }

    public LibroListVm List(string term = "", bool paging = false, int currentPage = 1)
    {
        var data = new LibroListVm();
        var list = _context.Libros!.ToList();

        if(!string.IsNullOrEmpty(term))
        {
            term = term.ToLower();
            list = list.Where(a => a.Titulo!.ToLower().Contains(term)).ToList();
        }

        if (paging)
        {
            int pageSize = 5;
            int count = list.Count;
            int TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            list = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            data.PageSize = pageSize;
            data.CurrentPage = currentPage;
            data.TotalPages = TotalPages;
        }

        // Obtener los nombres de las categorías asociadas a cada libro y mapear a LibroVm
        var vmList = new List<LibroVm>();
        foreach(var libro in list)
        {
            var categorias = (
                from categoria in _context.Categorias
                join lc in _context.LibroCategorias!
                on categoria.Id equals lc.CategoriaId
                where lc.LibroId == libro.Id
                select categoria.Nombre
            ).ToList();
            string categoriaNombres = string.Join(",", categorias); // drama, horror, accion
            
            vmList.Add(new LibroVm
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                Autor = libro.Autor,
                Descripcion = libro.Descripcion,
                Precio = libro.Precio,
                CreationDate = libro.CreationDate,
                Imagen = libro.Imagen,
                CategoriasNames = categoriaNombres
            });
        }

        data.LibroList = vmList;
        data.Term = term;
        return data;
    }

    public List<int> GetCategoriaByLibroId(int libroId)
    {
        return _context.LibroCategorias!.Where(lc => lc.LibroId == libroId).Select(lc => lc.CategoriaId).ToList();
    }
}