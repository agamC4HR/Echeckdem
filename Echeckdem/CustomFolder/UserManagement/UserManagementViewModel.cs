using Microsoft.AspNetCore.Mvc.Rendering;

namespace Echeckdem.CustomFolder.UserManagement
{
    public class UserManagementViewModel
    {
        public string UserID { get; set; }
        public string UNAME { get; set; }
        public string EmailID { get; set; }
        public string UserLevel { get; set; }
        public string OrganisationName { get; set; }
    }
}
