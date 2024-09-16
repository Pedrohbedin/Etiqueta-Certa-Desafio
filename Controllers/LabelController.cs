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

        [HttpPost]
        public IActionResult Create([FromBody] PostLabelViewModel labelViewModel)
        {
            var label = new Label
            {
                Name = labelViewModel.Name,
                Id_legislation = labelViewModel.Id_legislation
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

        [HttpPatch("{id:guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] PatchLabelViewModel labelViewModel)
        {
            try
            {
                _labelRepository.Update(labelViewModel, id);
                return Ok("Label atualizada com sucesso.");
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
