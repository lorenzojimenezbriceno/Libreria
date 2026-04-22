using Microsoft.AspNetCore.Mvc;
using AppStore.Repositories.Abstract;

namespace AppStore.Controllers;

public class HomeController : Controller
{
    private ILibroService _libroService;

    public HomeController(ILibroService libroService)
    {
        _libroService = libroService;
    }

    public IActionResult Index(string term = "", int currentPage = 1)
    {
        var list = _libroService.List(term, true, currentPage);

        return View(list);
    }

    public IActionResult LibroDetail(int libroId)
    {
        var libro = _libroService.GetById(libroId);
        if (libro == null) return NotFound();

        var categoriasIds = _libroService.GetCategoriaByLibroId(libroId);
        var dbContext = HttpContext.RequestServices.GetService<AppStore.Models.Context.DatabaseContext>();
        var categoriasNombres = dbContext?.Categorias?.Where(c => categoriasIds.Contains(c.Id)).Select(c => c.Nombre).ToList();
        
        var vm = new AppStore.Models.DTO.LibroVm
        {
            Id = libro.Id,
            Titulo = libro.Titulo,
            Descripcion = libro.Descripcion,
            Autor = libro.Autor,
            Precio = libro.Precio,
            CreationDate = libro.CreationDate,
            Imagen = libro.Imagen,
            CategoriasNames = categoriasNombres != null ? string.Join(", ", categoriasNombres) : ""
        };
        
        return View(vm);
    }


    public IActionResult About()
    {
        return View();
    }
}