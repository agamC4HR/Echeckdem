using Microsoft.AspNetCore.Mvc.Rendering;

namespace Echeckdem.CustomFolder
{
    public class BOCWEditViewModel
    {
        // NCBOCW
        public int TransactionID { get; set; }
        public string LCode { get; set; }
        public DateOnly DueDate { get; set; }
        public int Status { get; set; }
        public DateOnly? CompletionDate { get; set; }

        // NCACTION
        public int ACID { get; set; }
        public string? ACTitle { get; set; }         // Read-only
        public string? ACDetail { get; set; }
        public int? ACShow { get; set; }             
        public string? ACStatus { get; set; }        // Read-only
        public DateOnly? ACRDate { get; set; }       // Read-only
        public string? ACRemarks { get; set; }
        public DateOnly? ACIDate { get; set; }

        // File Upload
        public IFormFile? UploadedFile { get; set; }

        //STATUSMASTER
        public List<SelectListItem> AvailableStatuses { get; set; } = new();

        //NCACTAKEN

        public int? ACTID { get; set; }  
        public string? ActionTaken { get; set; }
        public DateOnly? ActionDate { get; set; }
        public DateOnly? ActionClosedDate { get; set; }
        public int? ShowClient { get; set; }
        public bool IsActTakenAvailable => ACTID.HasValue; 

    }
}
