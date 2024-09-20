using System.ComponentModel.DataAnnotations.Schema;

namespace Echeckdem.CustomFolder
{

    [Table("Ncmorg")]
    public class OrganisationList
    {
        public string Oname { get; set; }  

        public bool Oactive { get; set; }
        public string oid  { get; set; }
        public string spoc { get; set; }
        public int TotalCount { get; set; }
    }
}
