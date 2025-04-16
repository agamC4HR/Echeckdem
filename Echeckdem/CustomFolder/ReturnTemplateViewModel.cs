namespace Echeckdem.CustomFolder
{
    // SITE MANAGEMENT ---------------------- Model for NCTEMPRET used to map the returns to NCRET 
    public class ReturnTemplateViewModel
    {
        public int Rcode { get; set; }
        public string Rtitle { get; set; }
        public string Rform { get; set; }
        public string Rdesc { get; set; }
        public int? Rd { get; set; }
        public int? Rm { get; set; }
        public int? Yroff { get; set; }
        public bool Selected { get; set; } 
    }
}
