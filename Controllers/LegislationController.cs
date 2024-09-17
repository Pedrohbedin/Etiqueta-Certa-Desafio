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
        /// <summary>
        /// Função que adiciona um novo registro de uma Legislation ao banco de dados 
        /// </summary>
        /// <param name="legislationViewModel">Parametro que recebe os dados para cadastro de uma nova legislation</param>
        /// <returns>Retorna sucesso ou algum erro ocorrido no processo</returns>

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
        /// <summary>
        /// Função que lista o conjunto de legislations
        /// </summary>
        /// <returns>Retorna uma lista de legislations</returns>

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
