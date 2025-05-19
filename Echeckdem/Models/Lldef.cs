using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Lldef
{
    public string Acode { get; set; } = null!;

    public string Seccode { get; set; } = null!;

    public string? Section { get; set; }

    public string? Title { get; set; }

    public string? Dtext { get; set; }
}
