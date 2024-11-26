namespace Echeckdem.Models
{
    public class RegistrationViewModel
    {
        
        public string oid { get; set; }
        public string Lname { get; set; }
        public string LState { get; set; }
        public string State { get; set; }
        public string LCity { get; set; }  
        public string LRegion { get; set; }
        public string OName { get; set; }

        public DateTime? Doe { get; set; }
        public string TP { get; set; }
       public string Status { get; set; }

        public DateOnly? Dolr { get; set; }


        public string GetStatusDescription()
        {
            return Status switch
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
    
    }
}
