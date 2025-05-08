using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Ncaction
{
    public int Acid { get; set; }

    public string Oid { get; set; } = null!;

    public int? Aclink { get; set; }

    public string? Lcode { get; set; }

    public string? Actp { get; set; }

    public string? Actitle { get; set; }

    public string? Acdetail { get; set; }

    public int? Acshow { get; set; }

    public string? Acstatus { get; set; }

    public string? Acistatus { get; set; }

    public DateOnly? Acidate { get; set; }

    public DateOnly? Atargdate { get; set; }

    public DateOnly? Adocdate { get; set; }

    public DateOnly? Accldate { get; set; }

    public string? Acremarks { get; set; }

    public int? Acruser { get; set; }

    public DateOnly? Acrdate { get; set; }

    public string? Sbtp { get; set; }

    public string? Tpp { get; set; }
}
