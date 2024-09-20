using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Ncfile
{
    public int Fid { get; set; }

    public string Oid { get; set; } = null!;

    public string? Ftp { get; set; }

    public int? Flink { get; set; }

    public string? Fname { get; set; }

    public DateOnly? Fupdate { get; set; }

    public string? Ftitle { get; set; }

    public int? Farc { get; set; }

    public string? Lfile { get; set; }
}
