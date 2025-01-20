using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class BocwScope
{
    public string ScopeId { get; set; } = null!;

    public string ScopeName { get; set; } = null!;

    public int ScopeActive { get; set; }

    public virtual ICollection<BoScopeMap> BoScopeMaps { get; set; } = new List<BoScopeMap>();
}
