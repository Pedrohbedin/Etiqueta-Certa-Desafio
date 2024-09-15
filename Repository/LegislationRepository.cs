using WebApiEtiqueCerta.Interfaces;
using WebApiEtiqueCerta.Models;
using WebApiEtiqueCerta.ViewModels;
using Microsoft.EntityFrameworkCore;
using WebApiEtiqueCerta.ViewModels.Legislation;
using WebApiEtiqueCerta.ViewModels.ConservationProcesses;
using WebApiEtiqueCerta.ViewModels.Symbology;

namespace WebApiEtiqueCerta.Repository
{
    public class LegislationRepository : ILegislationRepository
    {
        etiquetaCertaContext ctx = new etiquetaCertaContext();
        public void Create(Legislation legislation)
        {
            ctx.Legislations.Add(legislation);
            ctx.SaveChanges(); 
        }

        public List<GetLegislationViewModel> GetAll()
        {
            var result = ctx.Legislations.Select(u => new GetLegislationViewModel
            {
                Id = u.Id,
                Name = u.Name,
                Official_language = u.Official_language,
                Conservation_process = ctx.ProcessInLegislations.Where(y => y.IdLegislation == u.Id).Select(p => new ConservationProcessesViewModel
                {
                    Id_process = p.IdProcess,
                    Symbology = ctx.SymbologyTranslates.Where(x => x.IdSymbologyNavigation!.IdProcess == p.IdProcess && x.IdLegislation == u.Id).Select(s => new SymbologyViewModel
                    {
                        Id = s.IdSymbology,
                        Translate = s.Translate
                    }).ToList(),
                }).ToList(),
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            }).ToList();

            return result;
        }

    }
}
