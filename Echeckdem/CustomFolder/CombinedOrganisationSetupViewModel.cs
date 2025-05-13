using Echeckdem.Controllers;
using Echeckdem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Echeckdem.CustomFolder
{
    public class CombinedOrganisationSetupViewModel
    {
        public List<OrganisationsListViewModel> OrganisationsList { get; set; }
        public OrganisationGeneralInfoViewModel? SelectedOrganisation { get; set; }

        public List<AddLocationViewModel> AddLocation { get; set; } = new List<AddLocationViewModel>();
        public List<SelectListItem>? SpocList { get; set; }
        //OrganisationsListViewModel
        public string oid { get; set; }
        public string Oname { get; set; }

        //OrganisationGeneralInfoViewModel  
        public string Spoc { get; set; }
        public int TotalCount { get; set; }

       
        public int? styear { get; set; }
        public string? Contname { get; set; }
        public string? Contemail { get; set; }
        public string? ImportData { get; set; }
        //
        public string? Lcode { get; set; }

        public string? Oid { get; set; }

        public string? Lname { get; set; }

        public string? Lcity { get; set; }

        public string? Lstate { get; set; }

        public string? Lregion { get; set; }
        public int? Lactive { get; set; }

        public int? Iscentral { get; set; }

        public int? Iscloc { get; set; }

        public string? Ltype { get; set; }

        public int? Lsetup { get; set; }
        
        public string LtypeDescription
        {
            get
            {
                var trimmedLtype = Ltype?.Trim().ToUpper();
                return trimmedLtype switch
                {
                    "S" => "SE",
                    "F" => "Fact",
                    "BO" => "BOCW",
                    
                    _ => "Unknown"
                };
            }
        }

    }
}
