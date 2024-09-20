using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class EnquiryTable
{
    public int Enid { get; set; }

    public string Ename { get; set; } = null!;

    public string Edesignation { get; set; } = null!;

    public string Eorganization { get; set; } = null!;

    public string Eemail { get; set; } = null!;

    public string Eintrest { get; set; } = null!;

    public string Ereference { get; set; } = null!;

    public DateOnly Edate { get; set; }

    public string? Emessage { get; set; }
}
