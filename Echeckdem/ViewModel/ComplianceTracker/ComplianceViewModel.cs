namespace Echeckdem.ViewModel.ComplianceTracker
{
    public class ComplianceViewModel
    {
        public int Id { get; set; }
        public string? Oid { get; set; }
        public string? Lcode { get; set; }
        public string? Oname { get; set; }

        public string? Lname { get; set; }
        public string? Lcity { get; set; }

        public string? Lstate { get; set; }
        public string? ServiceType { get; set; }

        
        public string? Service { get; set; }

        public string? Period { get; set; }

        public string? DueDate { get; set; }

        public string? CompletionDate { get; set; }
        public string? Status { get; set; }

        public string? FileName { get; set; }

    }
}
