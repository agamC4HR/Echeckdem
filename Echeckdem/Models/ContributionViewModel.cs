﻿using System.ComponentModel.DataAnnotations;

namespace Echeckdem.Models
{
    public class ContributionViewModel
    {
        // public int SerialNumber { get; set; }

        [Key]
        public int Contid { get; set; }
        [Key]
        public string Lcode { get; set; }

        [Key]
        public string oid { get; set; }
        public string Lname { get; set; }
        //public string LState { get; set; }

        public string State { get; set; }
        public string LCity { get; set; }
        public string LRegion { get; set; }
        //public string RTitle { get; set; }
        public string OName { get; set; }
        public DateOnly? LastDate { get; set; }
        public string TP {  get; set; }

        public int? Status { get; set; }

        public DateOnly? Depdate { get; set; }

        public DateOnly? chqdate { get; set; }

        public int Period { get; set; }
        public int Cyear { get; set; }

        public string? Remarks { get; set; }

        public string? Amount { get; set; }

        public string? Chqno { get; set; }

     

        public string FormattedPeriod
        {
            get
            {
                // Convert the Period (month number) into a month name
                DateTime date = new DateTime(Cyear, Period, 1);
                return date.ToString("MMMM-yyyy");
            }
        }

        public string GetStatusDescription()
        {
            return Status switch
            {

                0 => "Future",
                1 => "Compliant",
                2 => "Non Compliant",
                3 => "Not IN Scope",
                4 => "Not Applicable",
                5 => "Under Process",
                _ => "Unknown"
            };

        }
    }
}
