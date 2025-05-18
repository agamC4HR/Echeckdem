using Microsoft.AspNetCore.Mvc.Rendering;
using Echeckdem.Models;
using Echeckdem.CustomFolder;
namespace Echeckdem.ViewModel.ProjectBocw
    {
        public class BOCWEditViewModel

           {
     public ProjectSummary ProjectSummary { get; set; } 
        public NcbocwUpdateModel? Task { get; set; }
        public BOCWNcactionEdit? Ncaction { get;set; }
        public BocwNcactaken? Ncactaken { get; set; } = new();
        public List<BocwNcactakensum>? ncactakens { get;set; }

        
        


    }

    public class NcbocwUpdateModel
    {
        public List<SelectListItem>? AvailableStatuses { get; set; }
        public int TransactionId { get; set; }
        public string ScopeId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateOnly DueDate { get; set; }
        public int Status { get; set; }
        public DateOnly? CompletionDate { get; set; }
        public IFormFile? UploadedFile { get; set; }
        public string? ExistingFileName { get; set; }
    }
    public class BOCWNcactionEdit 
    {
        public int TransactionId { get; set; }
        public int Acid { get; set; }
        public string Oid { get; set; }
        public string Acdetail { get; set; }

        public string Acremarks { get; set; }
        public DateOnly Acidate { get; set; }
        public IFormFile? UploadedFile { get; set; }
        public List<string> ExistingFileName { get; set; } = new();
    }
    public class  BocwNcactaken
    {

        public int TransactionId { get; set; }

        public int Acid { get; set; }

        public DateOnly? Acdate { get; set; }

        public string? Actaken { get; set; }

        public DateOnly? Nacdate { get; set; }

       
    }
    public class BocwNcactakensum
    {
        public int Actid { get; set; }

        public int Acid { get; set; }

        public DateOnly? Acdate { get; set; }

        public string? Actaken { get; set; }

        public DateOnly? Nacdate { get; set; }

        public DateOnly? Accrdate { get; set; }

        public string? Uname { get; set; }


    }
}
