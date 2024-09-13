using Microsoft.AspNetCore.Mvc;
using WebApiEtiqueCerta.Interfaces;
using WebApiEtiqueCerta.Repository;
using WebApiEtiqueCerta.ViewModels;

namespace WebApiEtiqueCerta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class LabelController : Controller
    {
        private readonly ILabelRepository _context;

        public LabelController()
        {
            _context = new LabelRepository();
        }

        [HttpPost]
        public IActionResult Create([FromBody] PostLabelViewModel _label)
        {
            Label label = new Label
            {
                Name = _label.Name,
                Id_legislation = _label.Id_legislation
            };

            try
            {
                _context.Create(label);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        public IActionResult Update(Guid Id, Label label)
        {
            try
            {
                _context.Update(label, Id);
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
