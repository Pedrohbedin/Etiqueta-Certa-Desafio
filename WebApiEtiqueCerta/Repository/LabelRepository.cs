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

            try
            {
                ctx.Labels.Add(label);

                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Não foi possível adicionar o label ao banco de dados.", ex);
            }
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

            foreach (var item in label.Selected_symbology)
            {
                // Verifica se a SymbologyTranslate existe
                if (!symbologyTranslates.TryGetValue(item, out var symbologyTranslate))
                {
                    throw new Exception("SymbologyTranslate não encontrado para o Symbology e Legislation fornecidos.");
                }

                // Verifica se a LabelSymbology já existe
                var existingLabelSymbology = ctx.LabelSymbologies
                    .FirstOrDefault(ls => ls.IdLabel == labelToUpdate.Id && ls.IdSymbology == item);

                if (existingLabelSymbology != null)
                {
                    // Atualiza LabelSymbology se o processo for o mesmo
                    var existingSymbology = ctx.Symbologies.FirstOrDefault(x => x.Id == existingLabelSymbology.IdSymbology);
                    var newSymbology = ctx.Symbologies.FirstOrDefault(x => x.Id == item);

                    if (existingSymbology != null && newSymbology != null && existingSymbology.IdProcess == newSymbology.IdProcess)
                    {
                        existingLabelSymbology.IdSymbology = item;
                    }
                    else
                    {
                        // Adiciona uma nova LabelSymbology se o processo for diferente
                        newLabelSymbologies.Add(new LabelSymbology
                        {
                            IdLabel = labelToUpdate.Id,
                            IdSymbology = item
                        });
                    }
                }
                else
                {
                    // Adiciona uma nova LabelSymbology
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
