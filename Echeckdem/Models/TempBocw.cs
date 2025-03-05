using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class TempBocw
{
    public string Lcode { get; set; } = null!;

    public string ProjectCode { get; set; } = null!;

    public string? FileName { get; set; }

    public string? FilePath { get; set; }

    public DateOnly? UploadDate { get; set; }

    public string Oid { get; set; } = null!;

    public int UploadId { get; set; }
}
