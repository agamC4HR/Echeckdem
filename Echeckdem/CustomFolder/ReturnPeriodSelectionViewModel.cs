namespace Echeckdem.CustomFolder
{
    public class ReturnPeriodSelectionViewModel
    {
        public string Oid { get; set; }
        public string Lcode { get; set; }
        public string Lstate { get; set; }
        public string Ltype { get; set; }
        public int Iscloc { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }

        public List<ReturnTemplateViewModel> ApplicableReturns { get; set; }= new List<ReturnTemplateViewModel>();
    }
}
