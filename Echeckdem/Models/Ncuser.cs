using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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

    [NotMapped]
    public string? OName { get; set; } // Organization name (For Display)
    [NotMapped]
    public string? UserLevelName { get; set; } // User Level Name (For Display)
}
