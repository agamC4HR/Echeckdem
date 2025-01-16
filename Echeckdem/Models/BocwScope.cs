using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Echeckdem.Models;

public partial class BocwScope
{
    [Key]

    public string? ScopeId { get; set; }

    public string ScopeName { get; set; } = null!;

    public int ScopeActive { get; set; }
}
