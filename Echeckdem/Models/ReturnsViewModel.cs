using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Echeckdem.Models
{
        public class ReturnsViewModel
        {

        [Key]
        public int Rtid { get; set; }
        [Key]
        public string? Lcode { get; set; }

        [Key]
        public string? oid { get; set; }
            public string? Lname { get; set; }
            //public string LState { get; set; }
            public string? State { get; set; }
            public string? LCity { get; set; }
            public string? LRegion { get; set; }
            public string? RTitle { get; set; }
            public string? RForm { get; set; }
            public string? OName { get; set; }
            public DateOnly? LastDate { get; set; }
            public int? Status { get; set; }

            public DateOnly? Depdate {  get; set; }

            public string? Remarks { get; set; }

            public int RM {  get; set; }
            public int YROFF { get; set; }

        public string? Filename { get; set; }


        public DateTime? Period => CalculatePeriod();
            public string GetStatusDescription()
            {
                return Status switch
                {
                    0 => "Future",
                    1 => "Compliant",
                    2 => "Non Compliant",
                    3 => "Not IN Scope",
                    4 => "Not Applicable",
                    5 => "Under Process",
                    _ => "Unknown"
                };
            }

        public List<SelectListItem> StatusOptions { get; set; } = new List<SelectListItem>
    {
        new SelectListItem { Value = "0", Text = "Future" },
        new SelectListItem { Value = "1", Text = "Compliant" },
        new SelectListItem { Value = "2", Text = "Non Compliant" },
        new SelectListItem { Value = "3", Text = "Not IN Scope" },
        new SelectListItem { Value = "4", Text = "Not Applicable" },
        new SelectListItem { Value = "5", Text = "Under Process" }
    };





        private DateTime? CalculatePeriod()
            {
                if (LastDate.HasValue)
                {
                    // Get the current year
                    int currentYear = DateTime.Now.Year;

                    // Calculate the actual year based on YROFF
                    int year = currentYear + YROFF; // If YROFF is 0, year will be currentYear; if 1, it will be currentYear + 1

                    // Calculate the due date by adding RM months to the LastDate
                    DateTime dueDate = new DateTime(year, LastDate.Value.Month, 1).AddMonths(RM); // Assuming LastDate has a valid month
                    return dueDate;
                }

                // Return null if LastDate is not set
                return null;
            }

        }
    }
