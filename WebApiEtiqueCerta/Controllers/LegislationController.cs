using Microsoft.AspNetCore.Mvc;
using WebApiEtiqueCerta.Interfaces;
using WebApiEtiqueCerta.Models;
using WebApiEtiqueCerta.Repository;
using WebApiEtiqueCerta.ViewModels;

namespace WebApiEtiqueCerta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class LegislationController : Controller
    {
        private readonly ILegislationRepository _context;
        private readonly ISymbologyTranslateRepository _symbologyTranslateContext;
        private readonly IProcessInLegislation _processInLegislationContext;

        public LegislationController()
        {
            _context = new LegislationRepository();
            _symbologyTranslateContext = new SymbologyTranslateRepository();
            _processInLegislationContext = new ProcessInLegislationRepository();
        }

        [HttpPost]
        public IActionResult Create(LegislationViewModel _legislation)
        {
            try
            {
                Legislation legislation = new Legislation
                {
                    Name = _legislation.Name,
                    Official_language = _legislation.Official_language,
                };

                _context.Create(legislation);

                foreach (ConservationProcessesViewModel item in _legislation.Conservation_process!)
                {

                    ProcessInLegislation processInLegislation = new ProcessInLegislation
                    {
                        IdLegislation = legislation.Id,
                        IdProcess = item.Id_process
                    };

                    foreach (SymbologyViewModel symbology in item.Symbology!)
                    {
                        SymbologyTranslate symbologyTranslate = new SymbologyTranslate
                        {
                            IdSymbology = symbology.Id,
                            IdLegislation = legislation.Id,
                            Translate = symbology.Translate
                        };
                        if(!_symbologyTranslateContext.CheckExist(symbologyTranslate))
                        {
                            _symbologyTranslateContext.Create(symbologyTranslate);
                        }
                    };

                    _processInLegislationContext.Create(processInLegislation);
                }
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_context.GetAll());
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
