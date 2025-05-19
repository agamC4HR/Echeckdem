using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Echeckdem.Models;

public partial class Audtrail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Tindex { get; set; }

    public string? Uids { get; set; }

    public string? Origin { get; set; }

    public string? Activity { get; set; }

    public string? Ttable { get; set; }

    public string? Details { get; set; }

    public DateTime? Tdate { get; set; }

    public DateTime? Ttime { get; set; }

    public string? UserAgent { get; set; }

    public string? RequestPath { get; set; }
    public string? Country { get; set; }

    public string? City { get; set; }
    public string? SessionID { get; set; }

}
