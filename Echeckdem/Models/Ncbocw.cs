using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Ncbocw
{
    public string Lcode { get; set; } = null!;

    public string Lname { get; set; } = null!;

    public string ProjectCode { get; set; } = null!;

    public string ScopeId { get; set; } = null!;

    public string ScopeMapId { get; set; } = null!;

    public int WorkId { get; set; }

    public DateOnly DueDate { get; set; }

    public int Status { get; set; }

    public DateOnly? FirstAlert { get; set; }

    public DateOnly? FirstReminder { get; set; }

    public DateOnly? LastReminder { get; set; }

    public string Task { get; set; } = null!;

    public DateOnly CreateDate { get; set; }

    public int TransactionId { get; set; }

    public virtual Ncmloc LcodeNavigation { get; set; } = null!;

    public virtual Ncmlocbo ProjectCodeNavigation { get; set; } = null!;

    public virtual BocwScope Scope { get; set; } = null!;

    public virtual BoScopeMap ScopeMap { get; set; } = null!;

    public virtual TrackScope Work { get; set; } = null!;

    public DateOnly? CompletionDate { get; set; }
}
