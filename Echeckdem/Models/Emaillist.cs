using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Emaillist
{
    public string Email { get; set; } = null!;

    public string Category { get; set; } = null!;

    public bool Active { get; set; }
}
