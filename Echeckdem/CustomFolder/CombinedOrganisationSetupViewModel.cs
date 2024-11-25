using Echeckdem.Controllers;
using Echeckdem.Models;

namespace Echeckdem.CustomFolder
{
    public class CombinedOrganisationSetupViewModel
    {
        public List<OrganisationsListViewModel> OrganisationsList { get; set; }
        public OrganisationGeneralInfoViewModel? SelectedOrganisation { get; set; }
        //OrganisationsListViewModel
        public string oid { get; set; }
        public string Oname { get; set; }

        //OrganisationGeneralInfoViewModel
        public string Spoc { get; set; }
        public int TotalCount { get; set; }
        public int? styear { get; set; }
        public string? Contname { get; set; }
        public string? Contemail { get; set; }
        public string ImportData { get; set; }
    }
}
