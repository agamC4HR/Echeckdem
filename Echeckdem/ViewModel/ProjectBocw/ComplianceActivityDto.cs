using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Echeckdem.ViewModel.ProjectBocw
{
    public class ComplianceActivityDto
    {
        
        public int Acid { get; set; }
        public string ActivityType { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string StartDate { get; set; }
        public string Status { get; set; }
        public string CloseDate { get; set; }
        
    }
}
