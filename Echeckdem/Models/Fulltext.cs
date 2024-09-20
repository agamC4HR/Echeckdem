using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Fulltext
{
    public string Seccode { get; set; } = null!;

    public string? Act { get; set; }

    public string? Chapter { get; set; }

    public string? Section { get; set; }

    public string? Title { get; set; }

    public string? Ftext { get; set; }

    public double? Sindex { get; set; }
}
