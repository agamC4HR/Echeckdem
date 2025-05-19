using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Dorg
{
    public string Orgcode { get; set; } = null!;

    public string? Orgdesc { get; set; }

    public DateTime? Lstartdate { get; set; }

    public DateTime? Lenddate { get; set; }

    public int? Active { get; set; }

    public string? States { get; set; }
}
