using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class BoScopeMap
{
    public string ScopeMapId { get; set; } = null!;

    public string ScopeId { get; set; } = null!;

    public string Lcode { get; set; } = null!;

    public string? ProjectCode { get; set; }

    public bool Active { get; set; }

    public virtual Ncmloc LcodeNavigation { get; set; } = null!;

    public virtual Ncmlocbo? ProjectCodeNavigation { get; set; }

    public virtual BocwScope Scope { get; set; } = null!;

    public virtual BocwScope ScopeMap { get; set; } = null!;
}
