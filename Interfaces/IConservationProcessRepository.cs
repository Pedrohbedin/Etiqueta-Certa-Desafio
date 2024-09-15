namespace WebApiEtiqueCerta.Interfaces
{
    public interface IConservationProcessRepository
    {
        List<ConservationProcess> GetAll();
        void Create(ConservationProcess conservationProcess);
    }
}
