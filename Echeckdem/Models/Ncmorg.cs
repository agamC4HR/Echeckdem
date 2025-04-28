using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Ncmorg
{
    public string? SpocEml { get; set; }

    public string? Spoc { get; set; }

    public string Oid { get; set; } = null!;

    public string? Oname { get; set; }

    public int? Oactive { get; set; }

    public int? Otype { get; set; }

    public int? Styear { get; set; }

    public bool Echeck { get; set; }

    public bool Edoc { get; set; }

    public bool Vcheck { get; set; }

    public bool Client { get; set; }

    public bool Leed { get; set; }

    public string? Contname { get; set; }

    public string? Contemail { get; set; }

    public string? FileName { get; set; }
}
