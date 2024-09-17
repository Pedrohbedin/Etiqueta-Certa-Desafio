using System;
using System.Collections.Generic;

namespace WebApiEtiqueCerta.Models;

public partial class Legislation
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string? Name { get; set; }

    public string? OfficialLanguage { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual List<Label>? Labels { get; set; }

    public virtual List<ProcessInLegislation>? ProcessInLegislations { get; set; }

    public virtual List<SymbologyTranslate>? SymbologyTranslates { get; set; }
}
