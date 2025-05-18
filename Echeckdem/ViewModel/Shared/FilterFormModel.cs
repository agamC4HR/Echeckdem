using Microsoft.AspNetCore.Mvc.Rendering;

namespace Echeckdem.ViewModel.Shared
{
    public class FilterFormModel
    {
        public List<SelectListItem>? UClientList { get; set; }
        public List<SelectListItem>? USiteList { get; set; }
        public List<SelectListItem>? UStateList { get; set; }

        public List<SelectListItem>? UCityList { get; set; }

        public string? SelectedClient { get; set; }
        public string? SelectedSite { get; set; }
        public string? SelectedState { get; set; }
        public string? SelectedCity { get; set; }

        public DateOnly? StartDueDate { get; set; }
        public DateOnly? EndDueDate { get; set; }
        public DateOnly? StartPeriod { get; set; }
        public DateOnly? EndPeriod { get; set; }
    }
}
