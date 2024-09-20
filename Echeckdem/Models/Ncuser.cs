using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Ncuser
{
    public string Userid { get; set; } = null!;

    public int Uno { get; set; }

    public string? Oid { get; set; }

    public string? Uname { get; set; }

    public string? Password { get; set; }

    public int? Userlevel { get; set; }

    public int? Uactive { get; set; }

    public string? Stcode { get; set; }

    public string? Emailid { get; set; }
}
