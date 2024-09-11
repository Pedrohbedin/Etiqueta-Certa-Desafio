using WebApiEtiqueCerta.Models;

namespace WebApiEtiqueCerta.Interfaces
{
    public interface ILabelRepository
    {
        List<Label> GetAll();
        void Create(Label label);
        void Update(Label label, Guid id);
    }
}
