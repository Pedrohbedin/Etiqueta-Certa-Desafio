namespace WebApiEtiqueCerta.ViewModels.Label
{
    public class GetLabelViewModel
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public Guid? Id_legislation { get; set; }
        public List<SelectedSymbologyViewModel>? Selected_symbology { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
