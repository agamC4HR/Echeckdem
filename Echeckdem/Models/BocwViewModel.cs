using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace Echeckdem.Models
{
    public class BocwViewModel
    {
        public int TransactionID { get; set; }
        public string? Lcode { get; set; }
        //public string? oid { get; set; }
        public string? Lname { get; set; }


        public string? State { get; set; }
        public string? LCity { get; set; }
        public string? LRegion { get; set; }

        public string? OName { get; set; }

        public DateOnly? DueDate { get; set; }
        public DateOnly? CreateDate { get; set; }

        public int? Status { get; set; }

        public string? ScopeId { get; set; }

        //public string? ScopeName { get; set; }

        public string? Task {  get; set; }

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
