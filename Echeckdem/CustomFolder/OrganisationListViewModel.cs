using System.ComponentModel.DataAnnotations.Schema;

namespace Echeckdem.CustomFolder
{

    //[Table("Ncmorg")]
    public class OrganisationListViewModel
    {
        public string Oname { get; set; }
        public string oid { get; set; }
    }
    public class OrganisationGeneralInfoViewModel

    { 
        public string oid { get; set;  }

        public string Oname { get; set;  }
        public string Spoc { get; set; }
        public int TotalCount { get; set; }
        public int? styear { get; set; }
        public string? Contname { get; set; }
        public string? Contemail { get; set; }
    }
}
