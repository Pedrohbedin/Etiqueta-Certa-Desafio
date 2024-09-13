using System;
using System.Collections.Generic;

namespace WebApiEtiqueCerta.Models;

public partial class SymbologyTranslate
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid IdSymbology { get; set; }

    public Guid? IdLegislation { get; set; }

    public string? Translate { get; set; }

    public virtual Legislation? IdLegislationNavigation { get; set; }

    public virtual Symbology? IdSymbologyNavigation { get; set; }
}
