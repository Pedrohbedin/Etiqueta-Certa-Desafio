using WebApiEtiqueCerta.ViewModels.Symbology;

namespace WebApiEtiqueCerta.ViewModels.ConservationProcesses
{
    public class ConservationProcessesViewModel
    {
        public Guid Id_process { get; set; }
        public virtual List<SymbologyViewModel>? Symbology { get; set; }
    }
}
