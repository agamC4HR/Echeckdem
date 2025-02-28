using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class TrackScope
{
    public string ScopeId { get; set; } = null!;

    public string Task { get; set; } = null!;

    public string Stateid { get; set; } = null!;

    public int FirstAlert { get; set; }

    public int Reminder { get; set; }

    public int WorkId { get; set; }

    public virtual BocwScope? Scope { get; set; } = null!;
}
