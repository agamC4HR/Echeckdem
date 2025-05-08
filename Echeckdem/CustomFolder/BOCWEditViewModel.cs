namespace Echeckdem.CustomFolder
{
    public class BOCWEditViewModel
    {
        // NCBOCW
        public int TransactionID { get; set; }
        public string LCode { get; set; }
        public DateOnly DueDate { get; set; }
        public int Status { get; set; }
        public DateOnly? CompletionDate { get; set; }

        // NCACTION
        public int ACID { get; set; }
        public string? ACTitle { get; set; }         // Read-only
        public string? ACDetail { get; set; }
        public int? ACShow { get; set; }             // 0 = No, 1 = Yes
        public string? ACStatus { get; set; }        // Read-only
        public DateOnly? ACRDate { get; set; }       // Read-only
        public string? ACRemarks { get; set; }
        public DateOnly? ACIDate { get; set; }

        // File Upload
        public IFormFile? UploadedFile { get; set; }
    }
}
