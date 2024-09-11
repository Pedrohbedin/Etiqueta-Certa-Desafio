using System;
using System.Collections.Generic;

namespace WebApiEtiqueCerta.Models;

public partial class LabelSymbology
{
    public Guid Id { get; set; }

    public Guid? IdSymbology { get; set; }

    public Guid? IdLabel { get; set; }

    public virtual Label? IdLabelNavigation { get; set; }

    public virtual Symbology? IdSymbologyNavigation { get; set; }
}
