namespace Echeckdem.Models
{
    public class CombinedDetailedViewModel
    {
        public IEnumerable<RegistrationViewModel>? Registrations { get; set; }
        public IEnumerable<ContributionViewModel>? Contributions { get; set; }
        public IEnumerable<ReturnsViewModel>? Returns{ get; set; }

        //Filter Properties

        public string? OrganizationName { get; set; }
        public string? SiteName { get; set; }
        public string? StateName { get; set; }
        public string? CityName { get; set; }
        public DateOnly? StartDueDate { get; set; }
        public DateOnly? EndDueDate { get; set; }
        public DateOnly? StartPeriod { get; set; }
        public DateOnly? EndPeriod { get; set; }

       



    }
}
