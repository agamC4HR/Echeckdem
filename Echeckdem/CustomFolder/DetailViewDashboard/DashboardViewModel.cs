using Echeckdem.Models;

namespace Echeckdem.CustomFolder.DetailViewDashboard
{
    public class DashboardViewModel
    {
        public int OnTimeCount { get; set; }
        public List<ContributionViewModel> OnTimeContributions { get; set; } = new();
    }
}
