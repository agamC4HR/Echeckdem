using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Obligati
{
    public int Code { get; set; }

    public string? Act { get; set; }

    public string? Section { get; set; }

    public string? Title { get; set; }

    public string? Oblige { get; set; }

    public string? Type { get; set; }

    public string? Action { get; set; }

    public string? Timing { get; set; }

    public string? Audits { get; set; }

    public string? Support { get; set; }

    public string? Dsource { get; set; }

    public int? Parentcode { get; set; }

    public int? Oblindex { get; set; }

    public int? Riskrat { get; set; }

    public int? Oactive { get; set; }
}
