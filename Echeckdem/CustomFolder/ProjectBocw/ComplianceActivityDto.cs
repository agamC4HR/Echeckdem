using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Echeckdem.CustomFolder.ProjectBocw
{
    public class ComplianceActivityDto
    {
        public int SNo { get; set; }
        public int TransactionId { get; set; }
        public string ServiceType { get; set; }
        public string Service { get; set; }
        public string Category { get; set; }
        public DateOnly DueDate { get; set; }
        public string Status { get; set; }
        public DateOnly? CompletionDate { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}
