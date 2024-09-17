using WebApiEtiqueCerta.Interfaces;
using WebApiEtiqueCerta.ViewModels;
using Microsoft.EntityFrameworkCore;
using WebApiEtiqueCerta.ViewModels.Legislation;
using WebApiEtiqueCerta.ViewModels.ConservationProcesses;
using WebApiEtiqueCerta.ViewModels.Symbology;
using System.Data.Entity;
using System.Diagnostics;
using WebApiEtiqueCerta.Context;

namespace WebApiEtiqueCerta.Repository
{
    public class LegislationRepository : ILegislationRepository
    {
        etiquetaCertaContext ctx = new etiquetaCertaContext();

        /// <summary>
        /// Função que adiciona um novo registro de uma Legislation ao banco de dados 
        /// </summary>
        /// <param name="postLegislation">Parametro que recebe os dados que serão registrados ao banco</param>
        /// <exception cref="ArgumentException">Retorna do caso de preenchimento incorreto no payload</exception>
        public void Create(PostLegislationViewModel postLegislation)
        {
            Legislation legislation = new Legislation
            {
                Name = postLegislation.Name,
                OfficialLanguage = postLegislation.Official_language
            };

            ctx.Legislations.Add(legislation);

            HashSet<Guid> uniqueProcess = new HashSet<Guid>();

            foreach (var processViewModel in postLegislation.Conservation_process!)
            {
                if (uniqueProcess.Contains(processViewModel.Id_process))
                {
                    throw new ArgumentException("Processos duplicados, tente novamente");
                }

                uniqueProcess.Add(processViewModel.Id_process);

                ProcessInLegislation processInLegislation = new ProcessInLegislation
                {
                    IdLegislation = legislation.Id,
                    IdProcess = processViewModel.Id_process
                };

                HashSet<Guid> uniqueSymbologies = new HashSet<Guid>();

                foreach (var symbology in processViewModel.Symbology!)
                {
                    if (uniqueSymbologies.Contains(symbology.Id))
                    {
                        throw new ArgumentException("Simbologias duplicadas, tente novamente");
                    }

                    uniqueSymbologies.Add(symbology.Id);

                    var existingSymbologyTranslate = ctx.SymbologyTranslates
                        .FirstOrDefault(s => s.IdSymbology == symbology.Id && s.IdLegislation == legislation.Id);

                    var existingSymbology = ctx.Symbologies.FirstOrDefault(x => x.Id == symbology.Id);

                    if (existingSymbology == null || existingSymbology.IdProcess != processViewModel.Id_process)
                    {
                        throw new ArgumentException("Erro nos dados de entrada, simbologia não corresponde ao processo.");
                    }

                    if (existingSymbologyTranslate == null)
                    {
                        SymbologyTranslate symbologyTranslate = new SymbologyTranslate
                        {
                            IdSymbology = symbology.Id,
                            IdLegislation = legislation.Id,
                            Translate = symbology.Translate
                        };

                        ctx.SymbologyTranslates.Add(symbologyTranslate);
                    }
                }

                ctx.ProcessInLegislations.Add(processInLegislation);
            }

            ctx.SaveChanges();
        }


        /// <summary>
        /// Recupera todas as Legislations do banco de dados.
        /// </summary>
        /// <returns>Retorna uma lista de legislations</returns>
        public List<GetLegislationViewModel> GetAll()
        {
            var result = ctx.Legislations
                .Select(u => new GetLegislationViewModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    Official_language = u.OfficialLanguage,
                    Conservation_process = ctx.ProcessInLegislations
                        .Where(p => p.IdLegislation == u.Id)
                        .Select(p => new ConservationProcessesViewModel
                        {
                            Id_process = p.IdProcess,
                            Symbology = ctx.SymbologyTranslates
                                .Where(s => s.IdSymbologyNavigation!.IdProcess == p.IdProcess && s.IdLegislation == u.Id)
                                .Select(s => new SymbologyViewModel
                                {
                                    Id = s.IdSymbology,
                                    Translate = s.Translate
                                })
                                .ToList()
                        })
                        .ToList(),
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt
                })
                .ToList();

            return result;
        }


    }
}
