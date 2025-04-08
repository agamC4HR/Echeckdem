namespace Echeckdem.CustomFolder
{
    public class StateTemplateRetViewModel
    {
        public int Rcode { get; set; }

        public string Rstate { get; set; }

        public string Rtype { get; set; }

        public string Rtitle { get; set; }

        public string Rform { get; set; }

        public int? Rd {  get; set; }
        
        public int Rm { get; set; } 
        
        public int? Yroff { get; set; }  

        public int? Roblig { get; set; } 

        public string? Ract { get; set; }

        public int? Ractive { get; set; }

        public string RtypeDisplay => Rtype switch
        {
            "C" => "Common",
            "D" => "State",
            "S" => "S&E",
            "F" => "Factory",
            "P" => "CLRA PE",
            "I" => "ID Act",
            "E" => "EE Act",
            _ => Rtype
        };

        public string RmDisplay => new DateTime(2023, Rm, 1).ToString("MMMM");
        public string YroffDisplay => Yroff == 1 ? "Yes" : "No";
        public string RactiveDisplay => Ractive == 1 ? "Active" : "Inactive";


    }
}
