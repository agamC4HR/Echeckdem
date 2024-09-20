using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Mwnote
{
    public string Stid { get; set; } = null!;

    public string Catgrp { get; set; } = null!;

    public string? Title { get; set; }

    public string? Tp1 { get; set; }

    public string? Tp2 { get; set; }

    public string? Z1n { get; set; }

    public string? Z2n { get; set; }

    public string? Z3n { get; set; }

    public string? Stnotes { get; set; }

    public string? Stcats { get; set; }

    public string? Z4n { get; set; }
}
