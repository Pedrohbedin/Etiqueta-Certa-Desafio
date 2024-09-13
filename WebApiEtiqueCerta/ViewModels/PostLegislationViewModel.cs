using System.Text.Json.Serialization;

namespace WebApiEtiqueCerta.ViewModels
{
    public class PostLegislationViewModel
    {
        public string? Name { get; set; }
        public string? Official_language { get; set; }
        public List<ConservationProcessesViewModel>? Conservation_process { get; set; }
    }
}
