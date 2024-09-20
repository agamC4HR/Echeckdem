using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Ncactaken
{
    public int Actid { get; set; }

    public int Acid { get; set; }

    public DateOnly? Acdate { get; set; }

    public string? Actaken { get; set; }

    public DateOnly? Nacdate { get; set; }

    public int? Uno { get; set; }

    public DateOnly? Accrdate { get; set; }

    public int? Showclient { get; set; }
}
