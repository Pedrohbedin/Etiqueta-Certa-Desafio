using Microsoft.AspNetCore.Mvc;
using WebApiEtiqueCerta.Interfaces;
using WebApiEtiqueCerta.ViewModels.Label;
using System;
using WebApiEtiqueCerta.Repository;

namespace WebApiEtiqueCerta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class LabelController : ControllerBase
    {
        private readonly ILabelRepository _labelRepository;

        public LabelController()
        {
            _labelRepository = new LabelRepository();
        }

        /// <summary>
        /// Função que adiciona um novo registro de um Label ao banco de dados.
        /// </summary>
        /// <param name="getLabel">Parametro que recebe os dados para cadastro de uma nova label</param>
        /// <returns>Retorna sucesso no cadastro ou algum erro ocorrido no processo que será recebido do Labelrepository</returns>
        [HttpPost]
        public IActionResult Create([FromBody] PostLabelViewModel getLabel)
        {
            var label = new Label
            {
                Name = getLabel.Name,
                IdLegislation = getLabel.Id_legislation
            };

            try
            {
                _labelRepository.Create(label);
                return Ok("Label cadastrada com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        ///  Função que atualiza as sombologies associadas a uma dada label.
        /// </summary>
        /// <param name="id">Parametro que recebe o id da legilastion que será associada a uma label</param>
        /// <param name="patchLabel">Parametro que recebe as sombologies que serão atualizadas</param>
        /// <returns>Retorna sucesso ou um algum erro ocorrido no processo que será recebido do LabelRepository</returns>

        [HttpPatch("{id:guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] PatchLabelViewModel patchLabel)
        {
            try
            {
                _labelRepository.Update(patchLabel, id);
                return Ok("Label atualizada com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Função que lista o conjunto de labels e suas limbologies 
        /// </summary>
        /// <returns>Retorna uma lista de labels</returns>

        [HttpGet]
        public ActionResult GetAll()
        {
            try
            {
                var labels = _labelRepository.GetAll();
                return Ok(labels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
