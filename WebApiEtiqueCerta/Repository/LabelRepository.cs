using WebApiEtiqueCerta.Interfaces;
using WebApiEtiqueCerta.Models;
using WebApiEtiqueCerta.ViewModels;

namespace WebApiEtiqueCerta.Repository
{
    public class LabelRepository : ILabelRepository
    {
        etiquetaCertaContext ctx = new etiquetaCertaContext();

        public void Create(Label label)
        {
            ctx.Labels.Add(label);
            ctx.SaveChanges();
        }

        public List<GetLabelViewModel> GetAll()
        {
            return ctx.Labels.Select(u => new GetLabelViewModel
            {
                Id = u.Id,
                Name = u.Name,
                Id_legislation = u.Id_legislation,
                Selected_symbology = ctx.SymbologyTranslates.Where(x => x.IdLegislation == u.Id_legislation).Select(x => new SelectedSymbologyViewModel
                {
                    Id = x.Id,
                    Id_process = ctx.Symbologies.FirstOrDefault(z => z.Id == x.IdSymbology)!.IdProcess,
                    Translate = u.Name
                }).ToList()
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

        public void Update(Label label, Guid id)
        {
            Label _label = GetById(id);

            if(_label != null)
            {
                _label = label;

                ctx.SaveChanges();
            }
        }
    }
}
