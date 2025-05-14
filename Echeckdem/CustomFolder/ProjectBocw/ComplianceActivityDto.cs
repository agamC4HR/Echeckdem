namespace Echeckdem.CustomFolder.ProjectBocw
{
    public class ComplianceActivityDto
    {
        public int SNo { get; set; }
        public string ServiceType { get; set; } = "BOCW";
        public string Service { get; set; }
        public string DueDate { get; set; }
        public string Status { get; set; }
        public string CompletionDate { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }

        public int TransactionId { get; set; }
        public bool FileExists { get; set; }
    }
}
