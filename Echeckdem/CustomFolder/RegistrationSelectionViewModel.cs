namespace Echeckdem.CustomFolder
{
    public class RegistrationSelectionViewModel
    {
        public string Oid { get; set; }
        public string Lcode { get; set; }
        public string Ltype { get; set; }
        public string Lstate { get; set; }

        public int isesi { get; set; }

        public int ispf { get; set; }    


        public List<RegistrationTemplateViewModel> ApplicableRegistrations { get; set; } = new List<RegistrationTemplateViewModel>();
    }
}
