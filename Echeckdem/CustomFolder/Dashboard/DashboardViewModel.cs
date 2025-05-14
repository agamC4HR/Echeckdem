using Echeckdem.CustomFolder.Dashboard.Registration;
using Echeckdem.ViewModel;

namespace Echeckdem.CustomFolder.Dashboard
{
    public class DashboardViewModel
    {   
        //public int SelectedYear { get; set; }
        //public List<int> AvailableYears { get; set; } = new();

        public string ViewType { get; set; }

        public List<CompliantRegistrationViewModel> Registrations { get; set; }
        public List<ProjectDashboardStatus> ProjectDashboardStatus { get; set; }
        //public IEnumerable<CompliantRegistrationViewModel>? Registrations { get; set; }
        //public IEnumerable<RegistrationStatusViewModel>? ExpiringSites { get; set; }
        //public IEnumerable<RegistrationStatusViewModel>? ExpiredSites { get; set; }
    }
}
