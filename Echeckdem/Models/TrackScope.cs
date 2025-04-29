using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class TrackScope
{
    public string ScopeId { get; set; } = null!;

    public int WorkId { get; set; }

    public int? DueDate { get; set; }

    public string? Reference { get; set; }

    public virtual ICollection<Ncbocw>? Ncbocws { get; set; } = new List<Ncbocw>();

    public virtual BocwScope? Scope { get; set; } = null!;
}
