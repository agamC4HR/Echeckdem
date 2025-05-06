using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Echeckdem.CustomFolder
{
    public class TrackerFullViewModel
    {
        public IEnumerable<TrackerViewModel>? ActionViewModel { get; set; }
        public IEnumerable<TrackerTakenViewModel>? TakenViewModel { get; set; }
                
        public string? SelectedACTITLE { get; set; }
        public string? SelectedSBTP { get; set; }
        public string? SelectedTPP { get; set; }

        [BindNever]
        public string? Oname { get; set; }
        [BindNever]
        public string? Lname { get; set; }

        public int Acid { get; set; }
        public int Actid { get; set; }
        public DateOnly? Acdate { get; set; }
        public string Actaken { get; set; } = string.Empty;
        public DateOnly? Nacdate { get; set; }
        public int Showclient { get; set; }

        public int Uno { get; set; }

        
    }
}
