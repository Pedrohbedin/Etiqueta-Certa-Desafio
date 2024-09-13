using System.ComponentModel;
using System.Text.Json.Serialization;

namespace WebApiEtiqueCerta.ViewModels
{
    public class GetLegislationViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Official_language { get; set; }
        public List<ConservationProcessesViewModel>? Conservation_process { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
