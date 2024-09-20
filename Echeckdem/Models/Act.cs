using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Act
{
    public string Code { get; set; } = null!;

    public string? Lname { get; set; }

    public string? Sname { get; set; }

    public string? Year { get; set; }

    public string? Otherleg { get; set; }

    public string? Objectives { get; set; }

    public string? Applicability { get; set; }

    public string? Atype { get; set; }

    public string? Stcode { get; set; }

    public string? Lastamend { get; set; }

    public string? Alegtype { get; set; }

    public string? Parleg { get; set; }

    public int? Aactive { get; set; }

    public string? Legtype { get; set; }

    public string? Appl { get; set; }

    public int? Nodoc { get; set; }

    public int? Nochk { get; set; }

    public string? Filename { get; set; }
}
