using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Mwdatum
{
    public string Stid { get; set; } = null!;

    public int Catid { get; set; }

    public DateOnly Stdate { get; set; }

    public DateOnly? Endate { get; set; }

    public double? Basic { get; set; }

    public double? Da { get; set; }

    public double? Monthly { get; set; }

    public double? Daily { get; set; }

    public double? Z2b { get; set; }

    public double? Z2da { get; set; }

    public double? Z2m { get; set; }

    public double? Z2d { get; set; }

    public double? Z3b { get; set; }

    public double? Z3da { get; set; }

    public double? Z3m { get; set; }

    public double? Z3d { get; set; }

    public DateOnly? Notdate { get; set; }

    public string? Notfile { get; set; }

    public double? Hra { get; set; }

    public double? Spallowence { get; set; }

    public double? Z2hra { get; set; }

    public double? Z2spallowence { get; set; }

    public double? Z3hra { get; set; }

    public double? Z3spallowence { get; set; }

    public double? Z4b { get; set; }

    public double? Z4da { get; set; }

    public double? Z4hra { get; set; }

    public double? Z4spallowence { get; set; }

    public double? Z4m { get; set; }

    public double? Z4d { get; set; }

    public double? Other { get; set; }

    public double? Z2other { get; set; }

    public double? Z3other { get; set; }

    public double? Z4other { get; set; }
}
