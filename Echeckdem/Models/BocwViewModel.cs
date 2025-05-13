using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace Echeckdem.Models
{
    public class BocwViewModel
    {
        public int TransactionID { get; set; }
        public string? Lcode { get; set; }
        public string? oid { get; set; }
        public string? Lname { get; set; }


        public string? State { get; set; }
        public string? LCity { get; set; }
        public string? LRegion { get; set; }

        public string? OName { get; set; }

        public DateOnly? DueDate { get; set; }
        public DateOnly? CreateDate { get; set; }

        public int? Status { get; set; }

        public string? ScopeId { get; set; }

        //public string? ScopeName { get; set; }

       // public IFormFile? UploadedFile { get; set; }

        public string? Filename { get; set; }

        public DateOnly? CompletionDate { get; set; }

        public string? Task {  get; set; }

               public string GetStatusDescription()
        {
            return Status switch
            {

                0 => "Docs/Info Awaited",
                1 => "Under Processing",
                2 => "A/F",
                3 => "Received",
                4 => "Done",
                5 => "Not-Done",
                -1 => "Action Awaited",
                -2 => "Future",
                _ => "Unknown"
            };

        }

    }
}
