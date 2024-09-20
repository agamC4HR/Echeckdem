using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Noti
{
    public string Dcode { get; set; } = null!;

    public string Acode { get; set; } = null!;

    public string? Dtitle { get; set; }

    public DateTime? Ddate { get; set; }

    public string? Section { get; set; }

    public string? Ddesc { get; set; }

    public string? Detail { get; set; }

    public string? Folder { get; set; }

    public string? Filename { get; set; }

    public DateOnly? Crdate { get; set; }

    public int? Nactive { get; set; }

    public string? State { get; set; }

    public DateTime? Wefdate { get; set; }

    public string? Applictn { get; set; }

    public string? Exempt { get; set; }
}
