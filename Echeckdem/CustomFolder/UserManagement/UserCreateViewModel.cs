using System.ComponentModel.DataAnnotations;
namespace Echeckdem.CustomFolder.UserManagement

{
    public class UserCreateViewModel
    {
        
        public string UserID { get; set; }
               
        public string UNAME { get; set; }
        
        public string? Password { get; set; }
        
        public string? EmailID { get; set; }
               
        public int? UserLevel { get; set; }
               
        public string OID { get; set; }

        public int? Uactive { get; set; }
    }
}
