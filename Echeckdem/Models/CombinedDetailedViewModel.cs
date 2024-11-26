namespace Echeckdem.Models
{
    public class CombinedDetailedViewModel
    {
        public IEnumerable<RegistrationViewModel> Registrations { get; set; }
        public IEnumerable<ContributionViewModel> Contributions { get; set; }
        public IEnumerable<ReturnsViewModel> Returns{ get; set; }

        //Filter Properties

        public string OrganizationName { get; set; }
        public string SiteName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public DateTime? StartDueDate { get; set; }
        public DateTime? EndDueDate { get; set; }
        public DateTime? StartPeriod { get; set; }
        public DateTime? EndPeriod { get; set; }

        //Filter Properties

        public string OrganizationName { get; set; }

        public string SiteName { get; set; }

        public string StateName { get; set; }

        public string CityName { get; set; }



    }
}
