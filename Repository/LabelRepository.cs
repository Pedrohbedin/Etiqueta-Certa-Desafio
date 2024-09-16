using System.Linq;
using WebApiEtiqueCerta.Interfaces;
using WebApiEtiqueCerta.Models;
using WebApiEtiqueCerta.ViewModels;
using WebApiEtiqueCerta.ViewModels.Label;

namespace WebApiEtiqueCerta.Repository
{
    public class LabelRepository : ILabelRepository
    {
        etiquetaCertaContext ctx = new etiquetaCertaContext();

        public void Create(Label label)
        {
            if (label == null)
            {
                throw new ArgumentNullException(nameof(label), "O label não pode ser nulo.");
            }

            
            var existinglabel = ctx.Labels.FirstOrDefault(x => x.Id_legislation == label.Id_legislation);
            if(existinglabel != null)
            {
                throw new InvalidOperationException("Já existe uma label associada a está legislation");
            }

            ctx.Labels.Add(label);
            ctx.SaveChanges();
            
            
            
        }


        public List<GetLabelViewModel> GetAll()
        {
            // Recupera todos os dados necessários em uma única consulta
            var labels = ctx.Labels
                .Select(label => new
                {
                    label.Id,
                    label.Name,
                    label.Id_legislation,
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

            // Mapeia os dados para o modelo de visão
            return labels.Select(label => new GetLabelViewModel
            {
                Id = label.Id,
                Name = label.Name,
                Id_legislation = label.Id_legislation,
                Selected_symbology = label.Symbologies
                    .Select(symbology => new SelectedSymbologyViewModel
                    {
                        Id = symbology.IdSymbology,
                        Id_process = symbology.Symbology?.IdProcess ?? Guid.Empty,
                        Translate = symbology.Symbology?.Translate ?? string.Empty
                    })
                    .ToList()
            }).ToList();
        }


        public Label GetById(Guid id)
        {
            Label label = ctx.Labels.Find(id)!;

            if(label != null)
            {
                return label;
            }
            else
            {
                return null!;
            }
        }

        public void Update(PatchLabelViewModel label, Guid id)
        {
            // Recupera o Label a ser atualizado
            var labelToUpdate = ctx.Labels.FirstOrDefault(x => x.Id_legislation == id);
            if (labelToUpdate == null)
            {
                throw new Exception("Label não encontrado para o Id_legislation fornecido.");
            }

            // Atualiza o timestamp do Label
            labelToUpdate.UpdatedAt = DateTime.UtcNow;

            // Recupera a Legislation associada e atualiza o timestamp
            var legislation = ctx.Legislations.FirstOrDefault(l => l.Id == labelToUpdate.Id_legislation);
            if (legislation != null)
            {
                legislation.UpdatedAt = DateTime.UtcNow;
            }

            // Cria um dicionário para armazenar os SymbologyTranslates
            var symbologyTranslates = ctx.SymbologyTranslates
                .Where(st => label.Selected_symbology.Contains(st.IdSymbology)
                             && st.IdLegislation == labelToUpdate.Id_legislation)
                .ToDictionary(st => st.IdSymbology, st => st);

            // Cria uma lista para armazenar as novas LabelSymbologies
            var newLabelSymbologies = new List<LabelSymbology>();

            HashSet<Guid> uniqueSymbol = new HashSet<Guid>();

            foreach (var item in label.Selected_symbology)
            {
                // Verifica se o processo já foi adicionado no HashSet para garantir a unicidade
                if (uniqueSymbol.Contains(item))
                {
                    throw new ArgumentException("Simbologias duplicadas, tente novamente.");
                }

                // Adiciona o item ao HashSet
                uniqueSymbol.Add(item);

                // Verifica se a SymbologyTranslate existe
                if (!symbologyTranslates.TryGetValue(item, out var symbologyTranslate))
                {
                    throw new Exception("SymbologyTranslate não encontrado para o Symbology e Legislation fornecidos.");
                }

                // Verifica se a LabelSymbology já existe
                var existingLabelSymbology = ctx.LabelSymbologies
                    .FirstOrDefault(ls => ls.IdLabel == labelToUpdate.Id);

                // Se já existe a LabelSymbology, compara os processos
                if (existingLabelSymbology != null)
                {
                    // Busca a simbologia existente e a nova para comparar os processos
                    var existingSymbology = ctx.Symbologies.FirstOrDefault(x => x.Id == existingLabelSymbology.IdSymbology);
                    var newSymbology = ctx.Symbologies.FirstOrDefault(x => x.Id == item);

                    if (existingSymbology != null && newSymbology != null)
                    {
                        // Se o processo for o mesmo, atualiza a LabelSymbology
                        if (existingSymbology.IdProcess == newSymbology.IdProcess)
                        {
                            existingLabelSymbology.IdSymbology = item;  // Atualiza a simbologia
                            ctx.LabelSymbologies.Update(existingLabelSymbology);  // Atualiza o registro
                        }
                        else
                        {
                            // Se os processos forem diferentes, adiciona uma nova LabelSymbology
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
                    // Se a LabelSymbology não existe, adiciona uma nova
                    newLabelSymbologies.Add(new LabelSymbology
                    {
                        IdLabel = labelToUpdate.Id,
                        IdSymbology = item
                    });
                }
            }

            // Adiciona as novas LabelSymbologies ao contexto e salva as mudanças
            if (newLabelSymbologies.Any())
            {
                ctx.LabelSymbologies.AddRange(newLabelSymbologies);
            }

            ctx.SaveChanges();
        }
    }
}
