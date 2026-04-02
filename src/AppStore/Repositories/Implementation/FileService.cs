using AppStore.Repositories.Abstract;
using Microsoft.AspNetCore.Hosting;
using System.IO;


namespace AppStore.Repositories.Implementation;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment environment;

    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        environment = webHostEnvironment;
    }

    public Tuple<int, string> SaveImage(IFormFile imageFile)
    {
        try
        {
            // Obtenga el path para guardar el archivo y revise que exista
            string path = Path.Combine(environment.WebRootPath, "uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Revise que el archivo tenga una extensión válida
            var ext = Path.GetExtension(imageFile.FileName);
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            if (!allowedExtensions.Contains(ext.ToLower()))
            {
                return new Tuple<int, string>(400, "Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed.");
            }

            // Salve el archivo con un nombre único para evitar colisiones
            string uniqueFileName = $"{Guid.NewGuid()}{ext}";
            string fileWithPath = Path.Combine(path, uniqueFileName);
            using (var stream = new FileStream(fileWithPath, FileMode.Create))
            {  
                imageFile.CopyTo(stream);  
            }

            return new Tuple<int, string>(200, uniqueFileName);
        }
        catch (Exception ex)
        {
            // Handle exceptions and return appropriate status code and error message
            return new Tuple<int, string>(500, $"Error saving image: {ex.Message}");
        }
    }

    public bool DeleteFile(string imageFileName)
    {
        try
        {
            // Obtenga el path para guardar el archivo y revise que existe
            string path = Path.Combine(environment.WebRootPath, "uploads");
            if (!Directory.Exists(path))
            {
                Console.WriteLine($"Path doesn't exist: {path}");
                return false;
            }

            string filePath = Path.Combine(path, "uploads", imageFileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            else
            {
                Console.WriteLine($"File doesn't exist: {filePath}");
                return false;
            }       
        }
        catch (Exception ex)
        {
            // Handle exceptions if necessary (e.g., log the error)
            Console.WriteLine($"Error deleting file: {ex.Message}");
            return false;
        }
    }
}
