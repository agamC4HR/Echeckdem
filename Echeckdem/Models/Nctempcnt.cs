using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Nctempcnt
{
    public int Cid { get; set; }

    public string Cstate { get; set; } = null!;

    public string Tp { get; set; } = null!;

    public string Freq { get; set; } = null!;

    public int Period { get; set; }

    public int Ld { get; set; }

    public int? Moffset { get; set; }

    public int? Active { get; set; }
}
