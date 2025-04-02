using Microsoft.AspNetCore.Mvc.Rendering;

namespace Echeckdem.CustomFolder
{
    public class TrackerViewModel
    {
        public string SelectedOid { get; set; }

        public int SelectedUno { get; set; }

        public string SelectedLCODE { get; set; }
        public string SelectedTPP { get; set; }
        public string SelectedACTITLE { get; set; }
        public string SelectedSBTP { get; set; }

        public List<SelectListItem> Organizations { get; set; }
        public List<SelectListItem> Locations { get; set; }
        public List<SelectListItem> TPPDropdown { get; set; }
        public List<SelectListItem> ActDropdown { get; set; }
        public List<SelectListItem> SlaDropdown { get; set; }
    }
}
