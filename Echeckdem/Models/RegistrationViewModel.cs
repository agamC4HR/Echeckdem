using System.ComponentModel.DataAnnotations;

namespace Echeckdem.Models
{
    public class RegistrationViewModel
    {
        [Key]
        public int Uid { get; set; }
        [Key]
        public string? Lcode { get; set; }
        [Key]
        public string? Oid { get; set; }
        public string? Lname { get; set; }
        public string? LState { get; set; }
        public string? State { get; set; }
        public string? LCity { get; set; }  
        public string? LRegion { get; set; }
        public string? OName { get; set; }

        public string? Rno { get; set; }
        public int? Noe { get; set; }
        public string? Nmoe { get; set; }
        public DateOnly? Doi { get; set; }
        public DateOnly? Doe { get; set; }
        public string? TP { get; set; }
       public string? Status { get; set; }

        public DateOnly? Dolr { get; set; }

        public string? Remarks { get; set; }


        public string GetStatusDescription()
        {
            return Status?.Trim().ToUpper() switch
            {

                "E" => "Expired",
                "SU" => "Surrendered",
                "A" => "Applied",
                "NA" => "Not Applicable",
                "C" => "Compliant",
                "D" => "Documents Required",
                "N" => "Non-Compliant",
                "B" => "Awaiting Application",
                "AR" => "Ammendment Required",
                "AA" => "Ammendment Applied",
                "SC" => "Site Closed",
                "UP" => "Under Process",
                _ => "Unknown"
            };

        }

        public Dictionary<string, string> StatusOptions => new()                  // for dropdown in edit column
    {
        { "E", "Expired" },
        { "SU", "Surrendered" },
        { "A", "Applied" },
        { "NA", "Not Applicable" },
        { "C", "Compliant" },
        { "D", "Documents Required" },
        { "N", "Non-Compliant" },
        { "B", "Awaiting Application" },
        { "AR", "Amendment Required" },
        { "AA", "Amendment Applied" },
        { "SC", "Site Closed" },
        { "UP", "Under Process" }
    };
    }
}
