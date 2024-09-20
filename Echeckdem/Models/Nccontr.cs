using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Nccontr
{
    public int Contid { get; set; }

    public string Oid { get; set; } = null!;

    public string Lcode { get; set; } = null!;

    public string Tp { get; set; } = null!;

    public int Period { get; set; }

    public int Cyear { get; set; }

    public string? Freq { get; set; }

    public int? Ld { get; set; }

    public DateOnly? Lastdate { get; set; }

    public int? Status { get; set; }

    public string? Amount { get; set; }

    public DateOnly? Chqdate { get; set; }

    public string? Chqno { get; set; }

    public DateOnly? Depdate { get; set; }

    public int? Fileup { get; set; }

    public DateOnly? Uploaddate { get; set; }

    public string? Filename { get; set; }

    public string? Remarks { get; set; }
}
