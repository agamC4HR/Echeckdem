using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Ncreg
{
    public int Uid { get; set; }

    public string Oid { get; set; } = null!;

    public string Lcode { get; set; } = null!;

    public string Tp { get; set; } = null!;

    public string? Status { get; set; }

    public string? Rno { get; set; }

    public DateOnly? Doi { get; set; }

    public DateOnly? Doe { get; set; }

    public int? Noe { get; set; }

    public string? Nmoe { get; set; }

    public DateOnly? Dolr { get; set; }

    public string? Renhist { get; set; }

    public string? Filename { get; set; }

    public string? Remarks { get; set; }
}
