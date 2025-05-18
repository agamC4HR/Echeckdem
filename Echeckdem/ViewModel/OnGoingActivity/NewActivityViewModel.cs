using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace Echeckdem.ViewModel.OnGoingActivity
{
    public class NewActivityViewModel
    {
        [Required(ErrorMessage = "Please select a Client.")]
        public string? SelectedOid { get; set; }


        [Required(ErrorMessage = "Please select a Site.")]
        public string? SelectedLCODE { get; set; }

        [Required(ErrorMessage = "Please select an Authority.")]
        public string? SelectedTPP { get; set; }

        [Required(ErrorMessage = "Please select a Act.")]
        public string? SelectedACTITLE { get; set; }
        
        [Required(ErrorMessage = "Please select an Activity Type.")]
        public string? SelectedACTP { get; set; }

        public List<SelectListItem>? Organizations { get; set; }
        public List<SelectListItem>? Locations { get; set; }
        public List<SelectListItem>? TPPDropdown { get; set; }
        public List<SelectListItem>? ActDropdown { get; set; }
        public List<SelectListItem>? ACTPDropdown { get; set; }
    }
}
