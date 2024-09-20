using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Ncret
{
    public int Rtid { get; set; }

    public string Oid { get; set; } = null!;

    public string Lcode { get; set; } = null!;

    public int Ryear { get; set; }

    public int Rcode { get; set; }

    public DateOnly? Lastdate { get; set; }

    public int? Status { get; set; }

    public DateOnly? Depdate { get; set; }

    public int? Fileup { get; set; }

    public DateOnly? Uploaddate { get; set; }

    public string? Filename { get; set; }

    public string? Remarks { get; set; }
}
