using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Llauth
{
    public string Aucode { get; set; } = null!;

    public string? Acode { get; set; }

    public string? Auth { get; set; }

    public string? Duties { get; set; }

    public string? Powers { get; set; }

    public string? Jurisdiction { get; set; }

    public string? Section { get; set; }
}
