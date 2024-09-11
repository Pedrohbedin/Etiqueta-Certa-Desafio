using WebApiEtiqueCerta.Interfaces;

namespace WebApiEtiqueCerta.Repository
{
    public class ProcessInLegislationRepository : IProcessInLegislation
    {
        etiquetaCertaContext ctx = new etiquetaCertaContext();
        public void Create(ProcessInLegislation processInLegislation)
        {
            ctx.ProcessInLegislations.Add(processInLegislation);

            ctx.SaveChanges();
        }
    }
}
