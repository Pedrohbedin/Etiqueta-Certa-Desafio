using System.Linq;
using WebApiEtiqueCerta.Context;
using WebApiEtiqueCerta.Interfaces;
using WebApiEtiqueCerta.ViewModels;
using WebApiEtiqueCerta.ViewModels.Label;

namespace WebApiEtiqueCerta.Repository
{
    public class LabelRepository : ILabelRepository
    {
        etiquetaCertaContext ctx = new etiquetaCertaContext();

        /// <summary>
        /// Função que adiciona um novo registro de Label ao banco de dados
        /// </summary>
        /// <param name="label">Parametro recebido pela controller que será registrado</param>
        /// <exception cref="ArgumentNullException">Retorno do caso de o label ser nulo</exception>
        /// <exception cref="InvalidOperationException">Retorno do caso de já existir um label associado a legislation passada</exception>
        public void Create(Label label)
        {
            if (label == null)
            {
                throw new ArgumentNullException("O label não pode ser nulo.");
            }

            
            var existinglabel = ctx.Labels.FirstOrDefault(x => x.IdLegislation == label.IdLegislation);

            if(existinglabel != null)
            {
                throw new InvalidOperationException("Já existe uma label associada a está legislation");
            }

            ctx.Labels.Add(label);
            ctx.SaveChanges();
        }

        /// <summary>
        /// Recupera todas as Labels do banco de dados, incluindo suas simbologias associadas e traduções
        /// </summary>
        /// <returns>Retorna uma lista de Labels contendo seus detalhes e suas simbologias.</returns>
        public List<GetLabelViewModel> GetAll()
        {
            var labels = ctx.Labels
                .Select(label => new
                {
                    label.Id,
                    label.Name,
                    label.IdLegislation,
                    label.UpdatedAt,
                    label.CreatedAt,
                    Symbologies = ctx.LabelSymbologies
                        .Where(ls => ls.IdLabel == label.Id)
                        .Select(ls => new
                        {
                            ls.IdSymbology,
                            Symbology = ctx.Symbologies
                                .Where(s => s.Id == ls.IdSymbology)
                                .Select(s => new
                                {
                                    s.IdProcess,
                                    Translate = ctx.SymbologyTranslates
                                        .Where(st => st.IdSymbology == ls.IdSymbology)
                                        .Select(st => st.Translate)
                                        .FirstOrDefault()
                                })
                                .FirstOrDefault()
                        })
                        .ToList()
                })
                .ToList();

            return labels.Select(label => new GetLabelViewModel
            {
                Id = label.Id,
                Name = label.Name,
                Id_legislation = label.IdLegislation,
                Selected_symbology = label.Symbologies
                    .Select(symbology => new SelectedSymbologyViewModel
                    {
                        Id = symbology.IdSymbology,
                        Id_process = symbology.Symbology?.IdProcess ?? Guid.Empty,
                        Translate = symbology.Symbology?.Translate ?? string.Empty
                    })
                    .ToList(),
                UpdatedAt = label.UpdatedAt,
                CreatedAt = label.CreatedAt
            }).ToList();
        }

        /// <summary>
        /// Atualiza uma Label e suas propriedades
        /// </summary>
        /// <param name="label">Parametro que recebe os dados para atualizar a label</param>
        /// <param name="id">Parametro que recebe o id da legislation</param>
        /// <exception cref="Exception">Retorno do caso em que dados não foram encontrados</exception>
        /// <exception cref="ArgumentException">Retorna do caso em que existem dados duplicados no payload</exception>
        public void Update(PatchLabelViewModel label, Guid id)
        {
            var labelToUpdate = ctx.Labels.FirstOrDefault(x => x.IdLegislation == id);
            if (labelToUpdate == null)
            {
                throw new Exception("Label não encontrado para a legislação fornecida.");
            }

            labelToUpdate.UpdatedAt = DateTime.UtcNow;

            var newLabelSymbologies = new List<LabelSymbology>();

            HashSet<Guid> uniqueSymbol = new HashSet<Guid>();

            foreach (var item in label.Selected_symbology!)
            {
                var symbologyTranslates = ctx.SymbologyTranslates.FirstOrDefault(x => x.IdLegislation == id && x.IdSymbology == item);

                if (uniqueSymbol.Contains(item))
                {
                    throw new ArgumentException("Simbologies duplicados, tente novamente.");
                }

                uniqueSymbol.Add(item);

                if (symbologyTranslates == null)
                {
                    throw new Exception("SymbologyTranslate não encontrado");
                }

                var existingLabelSymbology = ctx.LabelSymbologies
                    .FirstOrDefault(ls => ls.IdLabel == labelToUpdate.Id);

                if (existingLabelSymbology != null)
                {
                    var existingSymbology = ctx.Symbologies.FirstOrDefault(x => x.Id == existingLabelSymbology.IdSymbology);

                    var newSymbology = ctx.Symbologies.FirstOrDefault(x => x.Id == item);

                    if (existingSymbology != null && newSymbology != null)
                    {
                        if (existingSymbology.IdProcess == newSymbology.IdProcess)
                        {
                            existingLabelSymbology.IdSymbology = item;
                            ctx.LabelSymbologies.Update(existingLabelSymbology);
                        }
                        else
                        {
                            newLabelSymbologies.Add(new LabelSymbology
                            {
                                IdLabel = labelToUpdate.Id,
                                IdSymbology = item
                            });
                        }
                    }
                }
                else
                {
                    newLabelSymbologies.Add(new LabelSymbology
                    {
                        IdLabel = labelToUpdate.Id,
                        IdSymbology = item
                    });
                }
            }

            if (newLabelSymbologies.Any())
            {
                ctx.LabelSymbologies.AddRange(newLabelSymbologies);
            }

            ctx.SaveChanges();
        }
    }
}
