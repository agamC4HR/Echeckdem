using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Reg
{
    public string Stid { get; set; } = null!;

    public string Tp { get; set; } = null!;

    public int Rindex { get; set; }

    public string? Rtitle { get; set; }

    public string? Ritem { get; set; }

    public string? Rfn { get; set; }

    public int? Ractive { get; set; }
}
