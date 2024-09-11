namespace WebApiEtiqueCerta.ViewModels
{
    public class ConservationProcessesViewModel
    {
        public Guid Id_process { get; set; }
        public virtual List<SymbologyViewModel>? Symbology { get; set; }
    }
}
