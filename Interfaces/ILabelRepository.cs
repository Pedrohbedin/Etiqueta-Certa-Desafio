using WebApiEtiqueCerta.Models;
using WebApiEtiqueCerta.ViewModels.Label;

namespace WebApiEtiqueCerta.Interfaces
{
    public interface ILabelRepository
    {
        List<GetLabelViewModel> GetAll();
        void Create(Label label);
        void Update(PatchLabelViewModel label, Guid id);
    }
}
