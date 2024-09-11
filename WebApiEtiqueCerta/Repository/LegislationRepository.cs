﻿using WebApiEtiqueCerta.Interfaces;
using WebApiEtiqueCerta.Models;
using WebApiEtiqueCerta.ViewModels;

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

        public List<LegislationViewModel> GetAll()
        {
            var result = ctx.Legislations.Select(u => new LegislationViewModel
            {
                Name = u.Name,
                Official_language = u.Official_language,
                Conservation_process = ctx.ProcessInLegislations.Where(y => y.IdLegislation == u.Id).Select(p => new ConservationProcessesViewModel
                {
                    Id_process = p.IdProcess,
                    Symbology = ctx.SymbologyTranslates.Where(x => x.IdSymbologyNavigation!.IdProcess == p.IdProcess && x.IdLegislation == u.Id).Select(s => new SymbologyViewModel
                    {
                        Id = s.IdSymbology,
                        Translate = s.Translate
                    }).ToList()
                }).ToList(),
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            }).ToList();

            return result;
        }

    }
}
