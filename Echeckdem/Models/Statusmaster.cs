using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Statusmaster
{
    public int Smid { get; set; }

    public string ScopeId { get; set; } = null!;

    public int Status { get; set; }

    public string Value { get; set; } = null!;

    public int Active { get; set; }

    public virtual BocwScope Scope { get; set; } = null!;
}
