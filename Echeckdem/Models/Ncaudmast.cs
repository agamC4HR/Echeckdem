using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Ncaudmast
{
    public int Aid { get; set; }

    public string Oid { get; set; } = null!;

    public string Lcode { get; set; } = null!;

    public string? Aperiod { get; set; }

    public int? Ayear { get; set; }

    public DateOnly? Adate { get; set; }

    public string? Aby { get; set; }

    public int? Auser { get; set; }

    public DateOnly? Aschdate { get; set; }

    public int? Atime { get; set; }

    public DateOnly? Arepdate { get; set; }

    public DateOnly? Acldate { get; set; }

    public int? Aclosed { get; set; }

    public double? Acomplete { get; set; }

    public double? Ascore { get; set; }

    public string? Noofemps { get; set; }

    public string? Audrep { get; set; }

    public string? Adremarks { get; set; }
}
