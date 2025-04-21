namespace Echeckdem.CustomFolder
{
    public class ContributionDetailViewModel
    {
        public string OrganizationName { get; set; }
        public string LocationName { get; set; }
        public string Freq { get; set; }
        public string Tp { get; set; }
        public string Period { get; set; }
        public int Cyear { get; set; }
        public DateOnly? LastDate { get; set; }

    }
}
