using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Echeckdem.ViewModel.OnGoingActivity
{
    public class TrackerFullViewModel
    {
        public string? Oid { get; set; }
        public string? ActivityType { get; set; }
        public int Acid { get; set; }
        public List<TrackerTakenViewModel>? TakenViewModel { get; set; }
        public AddNcactaken Taken { get; set; } = new();
        public string? Title { get; set; }
        public string Acstatus { get; set; }
        public List<SelectListItem> AcstatusList = new List<SelectListItem> { new SelectListItem {Text="Open",Value="O" },
        new SelectListItem { Value="I", Text="Docs received, In Process" },
    new SelectListItem { Value="D", Text="Docs requested" },
    new SelectListItem { Value="C", Text = "Closed" }
        };
        public string Acistatus { get; set; } 
        public List<SelectListItem> AcistatusList = new List<SelectListItem> { new SelectListItem {Text="Normal",Value="N" },
        new SelectListItem { Value="P", Text="Priority" },
    new SelectListItem { Value="E", Text="Escalated" },
    new SelectListItem { Value="C", Text = "Critical" }
        };
        public string? Acdetail { get; set; }

        public DateOnly? Acidate { get; set; }
        public DateOnly? Adocdate { get; set; }
        public DateOnly? Accldate { get; set; }

        public string? Remark { get; set; }

        public IFormFile? UploadedFile { get; set; }

        public List<string> ExistingFileName { get; set; } = new();

        public string? Oname { get; set; }

        public string? Lname { get; set; }

        public DateOnly? Acrdate { get; set; }

        public string? Uname { get; set; }

    }
    public class AddNcactaken
    {
        public int Acid { get; set; }
        public DateOnly? Acdate { get; set; }
        public string Actaken { get; set; } = string.Empty;
        public DateOnly? Nacdate { get; set; }
    }
    

}
