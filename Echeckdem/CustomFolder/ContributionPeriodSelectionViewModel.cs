namespace Echeckdem.CustomFolder
{
    public class ContributionPeriodSelectionViewModel
    {
        public string Oid { get; set; }
        public string Lcode { get; set; }
        public string Lstate { get; set; }
        
        public int Month { get; set; }
        public int Year { get; set; }

        public string SelectedTP { get; set; }

        

        public List<ContributionTemplateViewModel> ApplicableContributions { get; set; } = new List<ContributionTemplateViewModel>();
    }
}
