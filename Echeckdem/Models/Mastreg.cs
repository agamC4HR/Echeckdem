﻿using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Mastreg
{
    public string Rtype { get; set; } = null!;

    public string? Rdesc { get; set; }

    public string? Category { get; set; }

    public string? State { get; set; }
}
