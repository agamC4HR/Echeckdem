using Echeckdem.Controllers;
using Echeckdem.Models;

namespace Echeckdem.CustomFolder
{
    public class CombinedOrganisationSetupViewModel
    {
        public IEnumerable<OrganisationsListViewModel> OrganisationList { get; set; }
        public IEnumerable<OrganisationGeneralInfoViewModel> OrganisationGeneralInfo { get; set; }
        public IEnumerable<AddLocationViewModel> AddLocation { get; set; }

        //OrganisationsListViewModel
        public string oid { get; set; }

        public string Oname { get; set; }

        public string Spoc { get; set; }
        public int TotalCount { get; set; }
        public int? styear { get; set; }
        public string? Contname { get; set; }
        public string? Contemail { get; set; }

        public string ImportData { get; set; }
    }
}
