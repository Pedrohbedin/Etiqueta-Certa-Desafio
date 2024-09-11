using System;
using System.Collections.Generic;

namespace WebApiEtiqueCerta.Models;

public partial class Symbology
{
    public Guid Id { get; set; }

    public Guid IdProcess { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Url { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ConservationProcess? IdProcessNavigation { get; set; }

    public virtual ICollection<LabelSymbology>? LabelSymbologies { get; set; }

    public virtual List<SymbologyTranslate>? SymbologyTranslates { get; set; }
}
