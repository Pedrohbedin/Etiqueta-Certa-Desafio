using WebApiEtiqueCerta.Models;
using WebApiEtiqueCerta.ViewModels;

namespace WebApiEtiqueCerta.Interfaces
{
    public interface ILegislationRepository
    {
        List<GetLegislationViewModel> GetAll();
        void Create(Legislation legislation);

    }
}
