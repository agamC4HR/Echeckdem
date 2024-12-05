using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Echeckdem.Models;

public partial class Ncmloc
{
    [Key]
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

    public int? Lactive { get; set; }

    public int? Lsetup { get; set; }

    public string? FinEsclatnCnt { get; set; }

    public string? FinEsclatnMail { get; set; }

    public string? FinRespPrsn { get; set; }

    public string? FinRespEmail { get; set; }

    public string? Ltype { get; set; }

    public int? Iscloc { get; set; }
}
