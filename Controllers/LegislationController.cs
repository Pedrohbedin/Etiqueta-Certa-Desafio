using Microsoft.AspNetCore.Mvc;
using WebApiEtiqueCerta.Interfaces;
using WebApiEtiqueCerta.ViewModels.Legislation;
using System;
using WebApiEtiqueCerta.Repository;

namespace WebApiEtiqueCerta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class LegislationController : ControllerBase
    {
        private readonly ILegislationRepository _legislationRepository;

        public LegislationController()
        {
            _legislationRepository = new LegislationRepository();
        }

        [HttpPost]
        public IActionResult Create([FromBody] PostLegislationViewModel legislationViewModel)
        {
            try
            {
                _legislationRepository.Create(legislationViewModel);
                return Ok("Legislation cadastrada com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            try
            {
                var legislations = _legislationRepository.GetAll();
                return Ok(legislations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
