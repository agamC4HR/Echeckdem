using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Echeckdem.CustomFolder
{
    public class TrackerViewModel
    {


       // [BindNever]
        public string? SelectedOid { get; set; }

        public int SelectedUno { get; set; }

        //[BindNever]
        public string? SelectedLCODE { get; set; }

        //[BindNever]
        public string? SelectedTPP { get; set; }

        //[BindNever]
        public string? SelectedACTITLE { get; set; }

        //[BindNever]
        public string? SelectedSBTP { get; set; }

        public List<SelectListItem>? Organizations { get; set; }
        public List<SelectListItem>? Locations { get; set; }
        public List<SelectListItem>? TPPDropdown { get; set; }
        public List<SelectListItem>? ActDropdown { get; set; }
        public List<SelectListItem>? SlaDropdown { get; set; }

        [BindNever]
        public string? Oname { get; set; } // Full Organization Name
        [BindNever]
        public string? Lname { get; set; } // Full Location Name

        // Fields for Editing NCACTION
        public int Acid { get; set; }
        public string? Title { get; set; }
        public string? ExternalStatus { get; set; }
        public int? VisibleToClient { get; set; } = 0; // Default to 0 if null
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
