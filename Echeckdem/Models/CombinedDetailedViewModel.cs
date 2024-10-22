namespace Echeckdem.Models
{
    public class CombinedDetailedViewModel
    {
        public IEnumerable<RegistrationViewModel> Registrations { get; set; }
        public IEnumerable<ContributionViewModel> Contributions { get; set; }
        public IEnumerable<ReturnsViewModel> Returns{ get; set; }

        //Filter Properties

        public string OrganizationName { get; set; }

        public string Site {  get; set; }
        
        public string State { get; set; }
        public string City { get; set; }
        public string Category { get; set; }       // Reg, Contri, Ret

        
    }
}
