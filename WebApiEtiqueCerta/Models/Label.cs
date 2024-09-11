using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApiEtiqueCerta.Models;

public partial class Label
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string? Name { get; set; }

    public Guid? Id_legislation { get; set; }

    [JsonIgnore]
    public DateTime? CreatedAt { get; set; } = default(DateTime?);

    [JsonIgnore]
    public DateTime? UpdatedAt { get; set; } = default(DateTime?);

    [JsonIgnore]
    public virtual Legislation? IdLegislationNavigation { get; set; }

    public virtual ICollection<LabelSymbology> LabelSymbologies { get; set; } = new List<LabelSymbology>();
}
