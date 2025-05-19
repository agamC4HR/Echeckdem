using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Cuser
{
    public string Userid { get; set; } = null!;

    public int Uno { get; set; }

    public string? Uname { get; set; }

    public string? Uorg { get; set; }

    public string? Utype { get; set; }

    public int? Ulevel { get; set; }

    public string? Ustates { get; set; }

    public int? Uactive { get; set; }

    public string? Defstate { get; set; }

    public DateOnly? Crdate { get; set; }

    public DateOnly? Expdate { get; set; }

    public string? Mail { get; set; }

    public string? Password { get; set; }
}
