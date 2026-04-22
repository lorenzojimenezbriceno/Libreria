using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppStore.Repositories.Abstract;
using AppStore.Models.Domain;
using AppStore.Models.DTO;

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
        var vm = new LibroCreateVm
        {
            CategoriasList = _categoriaService.List()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Nombre
                }).ToList()
        };

        return View(vm);
    }

    [HttpPost]
    public IActionResult Add(LibroCreateVm vm)
    {
        vm.CategoriasList = _categoriaService.List()
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Nombre
            }).ToList();

        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var libro = new Libro
        {
            Titulo = vm.Titulo,
            Descripcion = vm.Descripcion,
            Autor = vm.Autor,
            Precio = vm.Precio,
            CreationDate = vm.CreationDate
        };

        if (vm.ImageFile != null)
        {
            var resultado = _fileService.SaveImage(vm.ImageFile); 
            if (resultado.Item1 != 200)
            {
                ModelState.AddModelError("", $"{resultado.Item1}, {resultado.Item2}");
                return View(vm);
            }
            
            libro.Imagen = resultado.Item2;
            
            var resultadoLibro = _libroService.Add(libro, vm.Categorias ?? new List<int>());
            if (!resultadoLibro)
            {
                // Borrar la imagen guardada si el libro no se pudo guardar
                _fileService.DeleteFile(resultado.Item2);
                ModelState.AddModelError("", "No se pudo guardar el libro. Se borró la imagen guardada.");
                return View(vm);
            }

            TempData["msg"] = "Se agrego el libro exitosamente.";
            return RedirectToAction(nameof(Add));
        }
        else
        {
            ModelState.AddModelError("", "La imagen es obligatoria.");
            return View(vm);
        }
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
        
        var vm = new LibroEditVm
        {
            Id = libro.Id,
            Titulo = libro.Titulo,
            Descripcion = libro.Descripcion,
            Autor = libro.Autor,
            Precio = libro.Precio,
            CreationDate = libro.CreationDate,
            Imagen = libro.Imagen,
            Categorias = categoriasDeLibro,
            MultiCategoriasList = new MultiSelectList(_categoriaService.List(), "Id", "Nombre", categoriasDeLibro)
        };

        return View(vm);
    }

    [HttpPost]
    public IActionResult Edit(LibroEditVm vm)
    {
        if (!ModelState.IsValid)
        {
            vm.MultiCategoriasList = new MultiSelectList(_categoriaService.List(), "Id", "Nombre", vm.Categorias);
            return View(vm);
        }

        var nombreNuevo = "";

        if (vm.ImageFile != null)
        {
            // Se salva la nueva imagen antes de actualizar el libro para asegurarse de que la imagen se guarda correctamente
            var resultado = _fileService.SaveImage(vm.ImageFile);
            nombreNuevo = resultado.Item2;
            if (resultado.Item1 != 200)
            {
                ModelState.AddModelError("", $"{resultado.Item1}, {resultado.Item2}");
                vm.MultiCategoriasList = new MultiSelectList(_categoriaService.List(), "Id", "Nombre", vm.Categorias);
                return View(vm);
            }
        }

        // Se borra el archivo de imagen anterior solo después de que la nueva imagen se ha guardado exitosamente
        if (!string.IsNullOrEmpty(vm.Imagen) && !string.IsNullOrEmpty(nombreNuevo))
        {
            var borro = _fileService.DeleteFile(vm.Imagen);
        }

        var libro = new Libro
        {
            Id = vm.Id,
            Titulo = vm.Titulo,
            Descripcion = vm.Descripcion,
            Autor = vm.Autor,
            Precio = vm.Precio,
            CreationDate = vm.CreationDate,
            Imagen = string.IsNullOrEmpty(nombreNuevo) ? vm.Imagen : nombreNuevo
        };
        
        var resultadoLibro = _libroService.Update(libro, vm.Categorias ?? new List<int>());
        if (!resultadoLibro)
        {
            // Borrar la imagen guardada si el libro no se pudo guardar
            if (!string.IsNullOrEmpty(nombreNuevo))
            {
                _fileService.DeleteFile(nombreNuevo);
            }
            ModelState.AddModelError("", "No se pudo guardar el libro.");
            vm.MultiCategoriasList = new MultiSelectList(_categoriaService.List(), "Id", "Nombre", vm.Categorias);
            return View(vm);
        }

        TempData["msg"] = "Se editó el libro exitosamente.";
        return RedirectToAction(nameof(LibroList));
    }

    public IActionResult LibroList()
    {
        var libros = _libroService.List();
        return View(libros);
    }

    public IActionResult Delete(int id)
    {
        _libroService.Delete(id);
        return RedirectToAction(nameof(LibroList));
    }
}