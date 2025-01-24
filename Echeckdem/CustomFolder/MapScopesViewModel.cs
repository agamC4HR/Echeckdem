using Echeckdem.Models;

namespace Echeckdem.CustomFolder
{
    public class MapScopesViewModel
    {
        public string Lcode { get; set; }
        public string ProjectCode { get; set; }
        public List<Ncmlocbo> Sites { get; set; }
        public List<BocwScope> Scopes { get; set; }
    }
}
