using WebApiEtiqueCerta.Interfaces;

namespace WebApiEtiqueCerta.Repository
{
    public class ConservationProcessRepository : IConservationProcessRepository
    {
        etiquetaCertaContext ctx = new etiquetaCertaContext();
        public void Create(ConservationProcess conservationProcess)
        {
            ctx.ConservationProcesses.Add(conservationProcess);
            ctx.SaveChanges();
        }

        public List<ConservationProcess> GetAll()
        {
            return ctx.ConservationProcesses.ToList();
        }
    }
}
