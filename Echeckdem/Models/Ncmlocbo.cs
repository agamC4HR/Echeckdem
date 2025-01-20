using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Ncmlocbo
{
    public string? Lcode { get; set; }

    public string ProjectCode { get; set; } = null!;

    public string? OvalId { get; set; }

    public string? ClientName { get; set; }

    public string? GeneralContractor { get; set; }

    public string? ProjectAddress { get; set; }

    public string NatureofWork { get; set; } = null!;

    public decimal ProjectArea { get; set; }

    public double? ProjectCostEst { get; set; }

    public DateOnly? ProjectStartDateEst { get; set; }

    public DateOnly? ProjectEndDateEst { get; set; }

    public int VendorCount { get; set; }

    public int WorkerHeadCount { get; set; }

    public string? ProjectLead { get; set; }

    public string? Lname { get; set; }

    public virtual ICollection<BoScopeMap> BoScopeMaps { get; set; } = new List<BoScopeMap>();

    public virtual Ncmloc? LcodeNavigation { get; set; }
}
