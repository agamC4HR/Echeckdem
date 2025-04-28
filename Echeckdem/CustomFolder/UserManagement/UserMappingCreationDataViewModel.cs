namespace Echeckdem.CustomFolder.UserManagement
{
    public class UserMappingCreationDataViewModel
    {
        public string UserId { get; set; }
        public int Uno { get; set; }
        public int? UserLevel { get; set; }
        public List<OrganisationViewModel> Organisations { get; set; }
    }
}
