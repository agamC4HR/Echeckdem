using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Audtrail
{
    public int Tindex { get; set; }

    public string? Uids { get; set; }

    public string? Origin { get; set; }

    public string? Activity { get; set; }

    public string? Ttable { get; set; }

    public string? Details { get; set; }

    public DateTime? Tdate { get; set; }

    public DateTime? Ttime { get; set; }
}
