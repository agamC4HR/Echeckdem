using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Llink
{
    public int Lid { get; set; }

    public string? Lcat { get; set; }

    public string? Ltitle { get; set; }

    public string? Ldesc { get; set; }

    public string? Lurl { get; set; }

    public int? Lactive { get; set; }

    public DateOnly? Crdate { get; set; }
}
