using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Lloff
{
    public string Offcode { get; set; } = null!;

    public string? Acode { get; set; }

    public string? Offence { get; set; }

    public string? Penalty { get; set; }

    public string? Section { get; set; }

    public int? Maxfine { get; set; }

    public int? Maxprison { get; set; }
}
