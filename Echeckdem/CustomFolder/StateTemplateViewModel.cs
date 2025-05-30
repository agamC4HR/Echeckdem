﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
namespace Echeckdem.CustomFolder
{
    public class StateTemplateViewModel
    {
        public int Cid { get; set; }
        public string CState { get; set; }

        
        public string StateName { get; set; }
        
        

        public string Tp { get; set; }
        public string Freq { get; set; }
        public int Period { get; set; }
        public int Ld { get; set; }
        public int? Moffset { get; set; }
        public int? Active { get; set; }

        // Display properties
        public string TpDisplay => Tp switch
        {
            "PT" => "Prof. Tax",
            "LWF" => "LWF",
            "PF" => "PF",
            "ESI" => "ESI",
            _ => Tp
            //_ => ""
        };

        public string FreqDisplay => Freq switch
        {
            "M" => "Monthly",
            "Q" => "Quarterly",
            "S" => "Half Yearly",
            "Y" => "Yearly",
            _ => Freq
            //_ => ""
        };

        public string PeriodDisplay => new DateTime(2023, Period, 1).ToString("MMMM");

        public string MoffsetDisplay => Moffset == 1 ? "Yes" : "No";
        public string ActiveDisplay => Active == 1 ? "Active" : "Inactive";
    }
}
