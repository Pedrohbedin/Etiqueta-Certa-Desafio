using WebApiEtiqueCerta.Interfaces;
using WebApiEtiqueCerta.Models;

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

        public List<Label> GetAll()
        {
            return ctx.Labels.Select(u => new Label
            {
                Id = u.Id,
                Name = u.Name,
                Id_legislation = u.Id_legislation,
                IdLegislationNavigation =  ctx.Legislations.FirstOrDefault(x => x.Id == u.Id_legislation),
                LabelSymbologies = u.LabelSymbologies
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
