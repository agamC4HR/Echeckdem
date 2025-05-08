namespace Echeckdem.CustomFolder
{
    public class TrackerTakenViewModel
    {
        public int Actid { get; set; } // Optional internal ID
        public int Acid { get; set; }  // Foreign key to NCACTION

        public DateOnly? Acdate { get; set; }
        public string Actaken { get; set; } = string.Empty;
        public DateOnly? Nacdate { get; set; }
        public int Showclient { get; set; }

        public int? Uno { get; set; }

        public string? Uname { get; set; }
    }
}
