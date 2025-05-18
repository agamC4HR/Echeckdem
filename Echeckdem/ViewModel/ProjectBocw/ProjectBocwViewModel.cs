using Echeckdem.ViewModel.OnGoingActivity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Echeckdem.ViewModel.ProjectBocw
{
    public class ProjectBocwViewModel
    {
        public List<SelectListItem> ClientList { get; set; }
        public List<SelectListItem> SiteList { get; set; }

        public ProjectDetailsDto projectDetailsDto { get; set; }
        public List<string> Clients { get; set; } = new List<string>();
        public Dictionary<string, List<string>> ClientSiteMap { get; set; } = new Dictionary<string, List<string>>();

        public List<TrackerViewModel> TrackerActions { get; set; }



    }
   
}
