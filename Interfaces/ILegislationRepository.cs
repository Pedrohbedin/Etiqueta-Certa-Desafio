using WebApiEtiqueCerta.Models;
using WebApiEtiqueCerta.ViewModels.Legislation;

namespace WebApiEtiqueCerta.Interfaces
{
    public interface ILegislationRepository
    {
        List<GetLegislationViewModel> GetAll();
        void Create(PostLegislationViewModel legislation);

    }
}
