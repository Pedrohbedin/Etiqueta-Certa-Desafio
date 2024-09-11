using System;
using System.Collections.Generic;

namespace WebApiEtiqueCerta.Models;

public partial class ProcessInLegislation
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid IdProcess { get; set; }

    public Guid? IdLegislation { get; set; }

    public virtual Legislation? IdLegislationNavigation { get; set; }

    public virtual ConservationProcess? IdProcessNavigation { get; set; }
}
