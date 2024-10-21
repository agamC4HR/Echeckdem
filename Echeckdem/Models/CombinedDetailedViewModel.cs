namespace Echeckdem.Models
{
    public class CombinedDetailedViewModel
    {
        public IEnumerable<RegistrationViewModel> Registrations { get; set; }
        public IEnumerable<ContributionViewModel> Contributions { get; set; }
        public IEnumerable<ReturnsViewModel> Returns{ get; set; }
    }
}
