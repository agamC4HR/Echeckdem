namespace Echeckdem.CustomFolder.ProjectBocw
{
    public class BocwServiceDto
    {
        public string ServiceType { get; set; } = "BOCW";
        public string Service { get; set; }
        public string Category { get; set; }
        public DateOnly DueDate { get; set; }
        public string Status { get; set; }
        public DateOnly? CompletionDate { get; set; }
        public string File { get; set; }

        public int? transactionID { get; set; }

        public string? lcode { get; set; }

        public string oid { get; set; }

    }
}
