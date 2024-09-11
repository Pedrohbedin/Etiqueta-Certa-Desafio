using System.ComponentModel;
using System.Text.Json.Serialization;

namespace WebApiEtiqueCerta.ViewModels
{
    public class LegislationViewModel
    {
        public string? Name { get; set; }
        public string? Official_language { get; set; }
        public List<ConservationProcessesViewModel>? Conservation_process { get; set; }
        [ReadOnly(true)]
        public DateTime? CreatedAt { get; set; }
        [ReadOnly(true)]
        public DateTime? UpdatedAt { get; set; }
    }
}
