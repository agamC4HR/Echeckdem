using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Mwcat
{
    public string Stid { get; set; } = null!;

    public int Catid { get; set; }

    public string Catgrp { get; set; } = null!;

    public string? Catname { get; set; }

    public string? Catdesc { get; set; }

    public int? Catlnk { get; set; }
}
