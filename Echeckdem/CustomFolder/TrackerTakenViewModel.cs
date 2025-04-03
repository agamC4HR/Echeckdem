namespace Echeckdem.CustomFolder
{
    public class TrackerTakenViewModel
    {
        public int Actid { get; set; }


        public int Acid { get; set; } // Foreign key from NCACTION
        public DateOnly? Acdate { get; set; } 
        public string Actaken { get; set; } 
        public DateOnly? Nacdate { get; set; } 
        public int Showclient { get; set; } 
    }
}
