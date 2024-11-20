using Echeckdem.Controllers;
using Echeckdem.Models;

namespace Echeckdem.CustomFolder
{
    public class CombinedOrganisationSetupViewModel
    {
        //public IEnumerable<OrganisationsListViewModel> OrganisationList { get; set; }
        //public OrganisationGeneralInfoViewModel OrganisationDetails { get; set; }
        ////public IEnumerable<OrganisationGeneralInfoViewModel> OrganisationDetails { get; set; }
        //public string CurrentFilter { get; set; }
        //public IEnumerable<AddLocationViewModel> AddLocation { get; set; }


        public List<OrganisationsListViewModel> OrganisationsList { get; set; }
        public OrganisationGeneralInfoViewModel? SelectedOrganisation { get; set; }
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
