using WebApiEtiqueCerta.Models;
using WebApiEtiqueCerta.ViewModels;

namespace WebApiEtiqueCerta.Interfaces
{
    public interface ILabelRepository
    {
        List<GetLabelViewModel> GetAll();
        void Create(Label label);
        void Update(Label label, Guid id);
    }
}
