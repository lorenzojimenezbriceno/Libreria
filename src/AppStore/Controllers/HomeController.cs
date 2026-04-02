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
        
        return View(libro);
    }


    public IActionResult About()
    {
        return View();
    }
}