using WebApiEtiqueCerta.Models;
using WebApiEtiqueCerta.ViewModels;

namespace WebApiEtiqueCerta.Interfaces
{
    public interface ILegislationRepository
    {
        List<LegislationViewModel> GetAll();
        void Create(Legislation legislation);

    }
}
