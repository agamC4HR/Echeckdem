using System;
using System.Collections.Generic;

namespace Echeckdem.Models;

public partial class Ncmloc
{
    public string Lcode { get; set; } = null!;

    public string Oid { get; set; } = null!;

    public string? Lname { get; set; }

    public string? Lstate { get; set; }

    public string? Lcity { get; set; }

    public string? Lregion { get; set; }

    public int? Iscentral { get; set; }

    public DateOnly? Doi { get; set; }

    public string? Laddress { get; set; }

    public string? Lcontact { get; set; }

    public string? Lconno { get; set; }

    public string? Lconemail { get; set; }

    public string? Cemail { get; set; }

    public string? Iemail { get; set; }

    public string? Lgroup2 { get; set; }

    public string? Lgroup3 { get; set; }

    public DateOnly? Stdate { get; set; }

    public DateOnly? Enddate { get; set; }

    public DateOnly? ContractExpiryDate { get; set; }

    public int? Lactive { get; set; }

    public int? Lsetup { get; set; }

    public string? FinEsclatnCnt { get; set; }

    public string? FinEsclatnMail { get; set; }

    public string? FinRespPrsn { get; set; }

    public string? FinRespEmail { get; set; }

    public string? Ltype { get; set; }

    public int? Iscloc { get; set; }

    public int? Ispf { get; set; }

    public int? Isesi { get; set; }

    public virtual ICollection<BoScopeMap> BoScopeMaps { get; set; } = new List<BoScopeMap>();

    public virtual ICollection<Ncbocw> Ncbocws { get; set; } = new List<Ncbocw>();

    public virtual ICollection<Ncmlocbo> Ncmlocbos { get; set; } = new List<Ncmlocbo>();
}
