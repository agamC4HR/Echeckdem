using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Nctempfin
{
    public int Rcode { get; set; }

    public string? Rstate { get; set; }

    public string? Rtype { get; set; }

    public int? Rd { get; set; }

    public int? Rm { get; set; }

    public int? Yroff { get; set; }

    public string? Rtitle { get; set; }

    public string? Rform { get; set; }

    public string? Rdesc { get; set; }

    public int? Roblig { get; set; }

    public string? Ract { get; set; }

    public int? Triggr { get; set; }

    public string? Frequency { get; set; }
}
