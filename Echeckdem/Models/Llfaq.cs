using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Llfaq
{
    public int Qcode { get; set; }

    public string? Ques { get; set; }

    public string? Qanswer { get; set; }

    public string? Cat1 { get; set; }

    public string? Cat2 { get; set; }

    public int? Qactive { get; set; }

    public DateOnly? Crdate { get; set; }
}
