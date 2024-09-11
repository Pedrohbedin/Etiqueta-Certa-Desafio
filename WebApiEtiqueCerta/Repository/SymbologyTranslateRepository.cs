using Microsoft.AspNetCore.Http.HttpResults;
using WebApiEtiqueCerta.Interfaces;
using WebApiEtiqueCerta.Models;

namespace WebApiEtiqueCerta.Repository
{
    public class SymbologyTranslateRepository : ISymbologyTranslateRepository
    {
        etiquetaCertaContext ctx = new etiquetaCertaContext();
        public void Create(SymbologyTranslate symbologyTranslate)
        {
            ctx.SymbologyTranslates.Add(symbologyTranslate);
            ctx.SaveChanges();
        }

        public List<SymbologyTranslate> GetAll()
        {
            return ctx.SymbologyTranslates.ToList();
        }

        public bool CheckExist(SymbologyTranslate symbologyTranslate)
        {
           if( ctx.SymbologyTranslates.FirstOrDefault(s => s.IdSymbology == symbologyTranslate.IdSymbology && s.IdLegislation == symbologyTranslate.IdLegislation) != null)
           {
                return true;
           }
           return false;
        }
    }
}
