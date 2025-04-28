using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class BocwScope
{
    public string ScopeId { get; set; } = null!;

    public string ScopeName { get; set; } = null!;

    public int ScopeActive { get; set; }

    public string? Category { get; set; }
    [NotMapped]
    public bool IsSelected { get; set; }

    public virtual ICollection<BoScopeMap> BoScopeMaps { get; set; } = new List<BoScopeMap>();

    public virtual ICollection<Ncbocw> Ncbocws { get; set; } = new List<Ncbocw>();

    public virtual ICollection<TrackScope> TrackScopes { get; set; } = new List<TrackScope>();
}
