using Echeckdem.Models;

namespace Echeckdem.CustomFolder
{
    public class DetailViewCombinedViewModel
    {
        public List<ContributionViewModel> Contributions { get; set; } = new();
        public List<ReturnsViewModel> Returns { get; set; } = new();
        public List<RegistrationViewModel> Registrations { get; set; } = new();
        public int Oid { get; set; }
        public DateOnly Depdate { get; set; }
        public int Status { get; set; }
        public DateOnly Lastdate { get; set; }
        public string Lname { get; set; }
        public string Lstate { get; set; }
        public string Lcity { get; set; }
        public string Lregion { get; set; }
        public string Oname { get; set; }
        public string State { get; set; }
        public string RecordType { get; set; }
    }
}
