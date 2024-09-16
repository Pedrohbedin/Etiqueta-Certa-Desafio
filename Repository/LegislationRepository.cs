﻿using WebApiEtiqueCerta.Interfaces;
using WebApiEtiqueCerta.Models;
using WebApiEtiqueCerta.ViewModels;
using Microsoft.EntityFrameworkCore;
using WebApiEtiqueCerta.ViewModels.Legislation;
using WebApiEtiqueCerta.ViewModels.ConservationProcesses;
using WebApiEtiqueCerta.ViewModels.Symbology;
using System.Data.Entity;
using System.Diagnostics;

namespace WebApiEtiqueCerta.Repository
{
    public class LegislationRepository : ILegislationRepository
    {
        etiquetaCertaContext ctx = new etiquetaCertaContext();
        public void Create(PostLegislationViewModel _legislation)
        {
            // Cria nova instância de Legislation e adiciona as informações básicas
            Legislation legislation = new Legislation
            {
                Name = _legislation.Name,
                Official_language = _legislation.Official_language
            };

            ctx.Legislations.Add(legislation);

            // Lista para armazenar todos os SymbologyTranslate adicionados para validação posterior
            List<SymbologyTranslate> symbologyTranslatesToValidate = new List<SymbologyTranslate>();

            // Itera pelos processos de conservação
            foreach (var processViewModel in _legislation.Conservation_process)
            {
                // Cria uma instância de ProcessInLegislation
                ProcessInLegislation processInLegislation = new ProcessInLegislation
                {
                    IdLegislation = legislation.Id,
                    IdProcess = processViewModel.Id_process
                };

                // HashSet para garantir que não haja duplicatas de simbologias dentro do mesmo processo
                HashSet<Guid> uniqueSymbologies = new HashSet<Guid>();

                // Itera pelas simbologias associadas ao processo de conservação
                foreach (var symbology in processViewModel.Symbology)
                {
                    // Verifica se a simbologia já foi adicionada no HashSet para esse processo
                    if (uniqueSymbologies.Contains(symbology.Id))
                    {
                        throw new ArgumentException("Simbologias duplicadas, tente novamente");
                    }

                    // Adiciona simbologia ao HashSet para garantir unicidade
                    uniqueSymbologies.Add(symbology.Id);

                    // Verifica se a tradução de simbologia já existe
                    var existingSymbologyTranslate = ctx.SymbologyTranslates
                        .FirstOrDefault(s => s.IdSymbology == symbology.Id && s.IdLegislation == legislation.Id);

                    var existingSymbology = ctx.Symbologies.FirstOrDefault(x => x.Id == symbology.Id);

                    if (existingSymbology == null || existingSymbology.IdProcess != processViewModel.Id_process)
                    {
                        throw new ArgumentException("Erro nos dados de entrada, simbologia não corresponde ao processo.");
                    }

                    if (existingSymbologyTranslate == null)
                    {
                        // Se não existir, cria um novo registro de SymbologyTranslate
                        SymbologyTranslate symbologyTranslate = new SymbologyTranslate
                        {
                            IdSymbology = symbology.Id,
                            IdLegislation = legislation.Id,
                            Translate = symbology.Translate
                        };

                        // Adiciona para validação posterior
                        symbologyTranslatesToValidate.Add(symbologyTranslate);

                        // Adiciona ao contexto
                        ctx.SymbologyTranslates.Add(symbologyTranslate);
                    }
                }

                // Adiciona a relação entre Legislation e Process
                ctx.ProcessInLegislations.Add(processInLegislation);
            }

            // Se todas as validações passarem, salva as alterações
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
