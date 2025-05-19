using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class TrackScope
{
    public string ScopeId { get; set; } = null!;

    public int WorkId { get; set; }

    public int? Offset { get; set; }

    public string? Reference { get; set; }
}
