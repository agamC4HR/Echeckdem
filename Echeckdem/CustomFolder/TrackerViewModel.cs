using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Echeckdem.CustomFolder
{
    public class TrackerViewModel
    {
        // User selections
        public string? SelectedOid { get; set; }
        public int SelectedUno { get; set; }
        public string? SelectedLCODE { get; set; }

        
        public string? SelectedTPP { get; set; }
        public string? SelectedACTITLE { get; set; }
        public string? SelectedSBTP { get; set; }

        public string? SelectedACTP { get; set; }
        // Dropdowns
        public List<SelectListItem>? Organizations { get; set; }
        public List<SelectListItem>? Locations { get; set; }
        public List<SelectListItem>? TPPDropdown { get; set; }
        public List<SelectListItem>? ActDropdown { get; set; }
        public List<SelectListItem>? SlaDropdown { get; set; }
        public List<SelectListItem>? ACTPDropdown {  get; set; }



        // Display values
        [BindNever]
        public string? Oname { get; set; }
        [BindNever]
        public string? Lname { get; set; }

        
        public int Acid { get; set; }
        public string? Title { get; set; }
        public string? ExternalStatus { get; set; }
        public int? VisibleToClient { get; set; } = 0;
        public string? InternalStatus { get; set; }
        public string? DetailOfIssue { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? DocsReceiptDate { get; set; }
        public DateOnly? CloseDate { get; set; }
        public string? Remarks { get; set; }

        [BindNever]
        public IFormFile? FileUpload { get; set; }

        [BindNever]
        public string? UploadedFileName { get; set; }
    }
}
