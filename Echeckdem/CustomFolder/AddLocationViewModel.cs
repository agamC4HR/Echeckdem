namespace Echeckdem.CustomFolder
{
    public class AddLocationViewModel
    {
        public string ImportData { get; set; }

        public string? Lcode { get; set; }

        public string? Oid { get; set; }

        public string? Lname { get; set; }

        public string? Lcity { get; set; }

        public string? Lstate { get; set; }

        public string? Lregion { get; set; }
        public int? Lactive { get; set; }

        public int? Iscentral { get; set; }

        public int? Iscloc { get; set; }

        public string? Ltype { get; set; }

        public int? Lsetup { get; set; }

        public string LtypeDescription
        {
            get
            {
                var trimmedLtype = Ltype?.Trim().ToUpper();
                return trimmedLtype switch
                    {
                     "S" => "SE",
                     "F" => "Fact",
                     "BO" => "BOCW",
                     _ => "Unknown"
                };
            }
        }
     
    }
}
