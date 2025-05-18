namespace Echeckdem.ViewModel.ProjectBocw
{
    public class ProjectDetailsDto
    {
        public string Oid { get; set; }
        public string Lcode { get; set; }
        public string SiteName { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ClientName { get; set; }
        public string GeneralContractor { get; set; }
        public string ProjectStartDate { get; set; }
        public string ProjectEndDate { get; set; }
        public decimal? ProjectArea { get; set; }
        public string ProjectCost { get; set; }
        public string ProjectLead { get; set; }

        public List<BocwServiceDto> BocwServices { get; set; }
        public List<ComplianceActivityDto> ProjectActivity { get; set; }
    }
}
