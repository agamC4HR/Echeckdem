namespace Echeckdem.ViewModel.OnGoingActivity
{
    public class TrackerTakenViewModel
    {
        public int Actid { get; set; }
        public int Acid { get; set; }
        public DateOnly? Acdate { get; set; }
        public string Actaken { get; set; } = string.Empty;
        public DateOnly? Nacdate { get; set; }
        public DateOnly? Acrdate { get; set; }

        public string? Uname { get; set; }
    }
}
