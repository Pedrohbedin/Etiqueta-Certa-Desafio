using Microsoft.AspNetCore.Mvc;
using WebApiEtiqueCerta.Interfaces;
using WebApiEtiqueCerta.Models;
using WebApiEtiqueCerta.Repository;
using WebApiEtiqueCerta.ViewModels.ConservationProcesses;
using WebApiEtiqueCerta.ViewModels.Legislation;
using WebApiEtiqueCerta.ViewModels.Symbology;

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
        public IActionResult Create([FromBody]PostLegislationViewModel _legislation)
        {
            try
            {
                _context.Create(_legislation);
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
