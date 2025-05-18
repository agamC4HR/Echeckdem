namespace Echeckdem.ViewModel.OnGoingActivity
{
    public class TrackerViewModel
    {
        public string? ActivityType { get; set; }
        public string? Oname { get; set; }
        public string? Lname { get; set; }
        public int Acid { get; set; }
        public string? Title { get; set; }
        public string? ExternalStatus { get; set; }
        
        public string? DetailOfIssue { get; set; }
        public DateOnly? StartDate { get; set; }
        
        public DateOnly? CloseDate { get; set; }



    }
}
