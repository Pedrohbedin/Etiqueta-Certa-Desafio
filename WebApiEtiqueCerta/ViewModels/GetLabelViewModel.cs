namespace WebApiEtiqueCerta.ViewModels
{
    public class GetLabelViewModel
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public Guid? Id_legislation { get; set; }
        public List<SelectedSymbologyViewModel>? Selected_symbology { get; set; }
    }
}
