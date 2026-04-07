using AppStore.Models.Domain;

namespace AppStore.Repositories.Abstract;

public interface ICategoriaService
{
    public IQueryable<Categoria> List();
    bool Add(Categoria categoria);    
    bool Delete(int id);
    bool Update(Categoria categoria);
    Categoria? GetById(int id);
}
