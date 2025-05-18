namespace Echeckdem.ViewModel.ProjectBocw
{
    public class BocwServiceDto
    {
        public int Id { get; set; }
        public string? Oname { get; set; }
        public string? Oid { get; set; }

        public string? Lcode { get; set; }

        public string? Lname { get; set; }
        public string? Lcity { get; set; }

        public string? Lstate { get; set; }

        //Also used for Project Tracker
        public string ServiceType { get; set; }
        public string Service { get; set; }
        public string Category { get; set; }
        public DateOnly DueDate { get; set; }
        public string Status { get; set; }
        public DateOnly? CompletionDate { get; set; }
        public string File { get; set; }

        public int? transactionID { get; set; }

       

    }
}
