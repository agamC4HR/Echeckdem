using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Echeckdem.CustomFolder
{
    public class OrganisationGeneralInfoViewModel
    {
        public string? oid { get; set; }
        
        public string Oname { get; set; }
        public string? Spoc { get; set; }

        public string? spoc_eml { get; set; }
        public int TotalCount { get; set; }                                         
        public int? styear { get; set; }
        public string? Contname { get; set; }
        public string? Contemail { get; set; }
        public int? Oactive { get; set; }
        
        public string? FileName { get; set; }
        public IFormFile? PdfFile { get; set; }
        public List<SelectListItem>? SpocList { get; set; }
    }
}
