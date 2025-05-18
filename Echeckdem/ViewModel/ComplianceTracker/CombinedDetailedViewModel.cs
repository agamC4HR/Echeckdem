using Echeckdem.CustomFolder;
using Echeckdem.CustomFolder.Dashboard.Registration;
using Echeckdem.Models;
using Echeckdem.ViewModel.ProjectBocw;
using Echeckdem.ViewModel.Shared;

namespace Echeckdem.ViewModel.ComplianceTracker
{
    public class CombinedDetailedViewModel
    {
        public List<ComplianceViewModel>? Registrations { get; set; }
        public List<ComplianceViewModel>? Contributions { get; set; }
        public List<ComplianceViewModel>? Returns{ get; set; }
        public List<BocwServiceDto>? BOCW { get; set; }

        public FilterFormModel? FilterFormModel { get; set; }



    }
}
