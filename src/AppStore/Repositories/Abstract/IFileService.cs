namespace AppStore.Repositories.Abstract;

public interface IFileService
{
    Tuple<int, string> SaveImage(IFormFile imageFile);

    bool DeleteFile(string imageFileName);
}
