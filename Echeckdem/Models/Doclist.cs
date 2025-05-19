using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Doclist
{
    public string Dcode { get; set; } = null!;

    public string Acode { get; set; } = null!;

    public string? Dtitle { get; set; }

    public string? Ddesc { get; set; }

    public string? Docname { get; set; }

    public string? Dirname { get; set; }

    public string? Dtype { get; set; }

    public string? Dcat { get; set; }

    public string? Oacode { get; set; }

    public int? Dindex { get; set; }
}
