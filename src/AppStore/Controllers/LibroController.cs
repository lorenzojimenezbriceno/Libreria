using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppStore.Repositories.Abstract;
using AppStore.Models.Domain;

namespace AppStore.Controllers;

[Authorize]
public class LibroController : Controller
{
    private readonly ILibroService _libroService;
    private readonly IFileService _fileService;
    private readonly ICategoriaService _categoriaService;
    
    public LibroController(
        ILibroService libroService, 
        IFileService fileService,
        ICategoriaService categoriaService)
    {
        _libroService = libroService;
        _fileService = fileService;
        _categoriaService = categoriaService;
    }

    public IActionResult Add()
    {
        var libro = new Libro
        {
            CategoriasList = _categoriaService.List()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Nombre
                }).ToList()
        };

        TempData["msg"] = null;
            
        return View(libro);
    }

    [HttpPost]
    public IActionResult Add(Libro libro)
    {
        libro.CategoriasList = _categoriaService.List()
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Nombre
            }).ToList();

        if (!ModelState.IsValid)
        {
            return View(libro);
        }

        if (libro.ImageFile != null)
        {
            var resultado = _fileService.SaveImage(libro.ImageFile); 
            if (resultado.Item1 != 200)
            {
                TempData["msg"] = $"{resultado.Item1}, {resultado.Item2}";
                return View(libro);
            }
            else
            {
                libro.Imagen = resultado.Item2;
                var resultadoLibro = _libroService.Add(libro);
                if (!resultadoLibro)
                {
                    // Borrar la imagen guardada si el libro no se pudo guardar
                    _fileService.DeleteFile(resultado.Item2);
                    TempData["msg"] = $"No se pudo guardar el libro. Se borró la imagen guardada.";
                    return RedirectToAction(nameof(Add));
                }

                TempData["msg"] = $"Se agrego el libro exitosamente.";
                return View(libro);
            }
        }

        return View();
    }

    public IActionResult Edit(int id)
    {
        var libro = _libroService.GetById(id);
        if (libro == null)
        {
            TempData["msg"] = $"No se encontró el libro con ID {id}.";
            return RedirectToAction(nameof(LibroList));
        }

        var categoriasDeLibro = _libroService.GetCategoriaByLibroId(id);
        var multiSelectListCategorias = new MultiSelectList(_categoriaService.List(), "Id", "Nombre", categoriasDeLibro);
        libro.MultiCategoriasList = multiSelectListCategorias;

        TempData["msg"] = null;
        
        return View(libro);
    }

    [HttpPost]
    public IActionResult Edit(Libro libro)
    {
        if (!ModelState.IsValid)
        {
            return View(libro);
        }

        var categoriasDeLibro = _libroService.GetCategoriaByLibroId(libro.Id);
        var multiSelectListCategorias = new MultiSelectList(_categoriaService.List(), "Id", "Nombre", categoriasDeLibro);
        libro.MultiCategoriasList = multiSelectListCategorias;

        var nombreNuevo = "";

        if (libro.ImageFile != null)
        {
            // Se salva la nueva imagen antes de actualizar el libro para asegurarse de que la imagen se guarda correctamente
            var resultado = _fileService.SaveImage(libro.ImageFile);
            nombreNuevo = resultado.Item2;
            if (resultado.Item1 != 200)
            {
                TempData["msg"] = $"{resultado.Item1}, {resultado.Item2}";
                return View(libro);
            }
        }

        // Se borra el archivo de imagen anterior solo después de que la nueva imagen se ha guardado exitosamente
        if (!string.IsNullOrEmpty(libro.Imagen) && !string.IsNullOrEmpty(nombreNuevo))
        {
            var borro = _fileService.DeleteFile(libro.Imagen);
        }

        libro.Imagen = nombreNuevo;
        var resultadoLibro = _libroService.Update(libro);
        if (!resultadoLibro)
        {
            // Borrar la imagen guardada si el libro no se pudo guardar
            _fileService.DeleteFile(nombreNuevo);
            TempData["msg"] = $"No se pudo guardar el libro. Se borró la imagen guardada.";
            return RedirectToAction(nameof(Edit));
        }

        TempData["msg"] = $"Se editó el libro exitosamente.";
        return View(libro);
    }

    public IActionResult LibroList()
    {
        var libros = _libroService.List();

        TempData["msg"] = null;
        
        return View(libros);
    }

    public IActionResult Delete(int id)
    {
        _libroService.Delete(id);
        return RedirectToAction(nameof(LibroList));
    }
}