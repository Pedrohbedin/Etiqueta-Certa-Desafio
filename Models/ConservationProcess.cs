using System;
using System.Collections.Generic;

namespace WebApiEtiqueCerta.Models;

public partial class ConservationProcess
{
    public Guid Id { get; set; } 

    public string? Name { get; set; }

    public DateTime? CreatedAt { get; set; } = default(DateTime?);

    public DateTime? UpdatedAt { get; set; } = default(DateTime?);

    public virtual ICollection<ProcessInLegislation>? ProcessInLegislations { get; set; }

    public virtual ICollection<Symbology>? Symbologies { get; set; }
}
