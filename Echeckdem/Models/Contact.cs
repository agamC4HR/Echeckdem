using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Contact
{
    public string Userid { get; set; } = null!;

    public string? Ccode { get; set; }

    public string? Fname { get; set; }

    public string? Lname { get; set; }

    public string? Orgcode { get; set; }

    public string? Location { get; set; }

    public string? Phone { get; set; }

    public string? Fax { get; set; }

    public string? Emailid { get; set; }

    public string? Altemail { get; set; }

    public string? Desig { get; set; }

    public string? Pass { get; set; }

    public decimal? Userlevel { get; set; }

    public decimal? Uactive { get; set; }

    public DateTime? Lastpass { get; set; }

    public string? Stcode { get; set; }

    public string? Fullname { get; set; }
}
