namespace WebApiEtiqueCerta.Interfaces
{
    public interface ISymbologyTranslateRepository
    {
        List<SymbologyTranslate> GetAll();
        void Create(SymbologyTranslate symbologyTranslate);
        bool CheckExist(SymbologyTranslate symbologyTranslate);
    }
}
