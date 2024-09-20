using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Maststate
{
    public string Stateid { get; set; } = null!;

    public string? Statedesc { get; set; }

    public string? Stactive { get; set; }
}
