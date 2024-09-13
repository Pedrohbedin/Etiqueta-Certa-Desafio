using Microsoft.AspNetCore.Http.HttpResults;
using System.Linq;
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

        public bool CheckExist(SymbologyTranslate symbologyTranslate, Guid idProcess)
        {
            //Rever aqui
            SymbologyTranslate _symbologyTranslate = ctx.SymbologyTranslates.FirstOrDefault(s => s.IdSymbology == symbologyTranslate.IdSymbology)!;

            if (_symbologyTranslate != null)
            {
                if (ctx.Symbologies.FirstOrDefault(x => x.Id == _symbologyTranslate.IdSymbology)!.IdProcess != idProcess)
                {
                    return true;
                }
            }

            if (_symbologyTranslate != null)
            {
                if (_symbologyTranslate.IdLegislation == symbologyTranslate.IdLegislation)
                {

                    return true;

                }
            }

            return false;
        }
    }
}
