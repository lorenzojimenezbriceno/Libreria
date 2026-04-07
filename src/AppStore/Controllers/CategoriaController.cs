using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppStore.Repositories.Abstract;
using AppStore.Models.Domain;
using AppStore.Models.DTO;

namespace AppStore.Controllers;

[Authorize]
public class CategoriaController : Controller
{
    private readonly ICategoriaService _categoriaService;
    
    public CategoriaController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    public IActionResult Add()
    {
        var categoria = new Categoria();

        TempData["msg"] = null;
            
        return View(categoria);
    }

    [HttpPost]
    public IActionResult Add(Categoria categoria)
    {
        if (!ModelState.IsValid)
        {
            return View(categoria);
        }

        var resultadoCategoria = _categoriaService.Add(categoria);
        if (!resultadoCategoria)
        {
            TempData["msg"] = $"No se pudo guardar la categoría.";
            return RedirectToAction(nameof(Add));
        }

        TempData["msg"] = $"Se agregó la categoría éxitosamente.";
        return View(categoria);
    }

    public IActionResult Edit(int id)
    {
        var categoria = _categoriaService.GetById(id);
        if (categoria == null)
        {
            TempData["msg"] = $"No se encontró la categoria con ID {id}.";
            return RedirectToAction(nameof(CategoriaList));
        }

        TempData["msg"] = null;
        return View(categoria);
    }

    [HttpPost]
    public IActionResult Edit(Categoria categoria)
    {
        if (!ModelState.IsValid)
        {
            return View(categoria);
        }

        var resultadoCategoria = _categoriaService.Update(categoria);
        if (!resultadoCategoria)
        {
            TempData["msg"] = $"No se pudo guardar la categoria.";
            return RedirectToAction(nameof(Edit));
        }

        TempData["msg"] = $"Se editó la categoría exitosamente.";
        return RedirectToAction(nameof(CategoriaList));
    }

    public IActionResult CategoriaList()
    {
        var categorias = _categoriaService.List();
        var categoriaListVm = new CategoriaListVm
        {
            CategoriasList = categorias,
        };
        return View(categoriaListVm);
    }

    public IActionResult Delete(int id)
    {
        // Borrar la categoria
        _categoriaService.Delete(id);
        
        // Borrar las categorias existentes de libreoCategorias
        
        return RedirectToAction(nameof(CategoriaList));
    }
}