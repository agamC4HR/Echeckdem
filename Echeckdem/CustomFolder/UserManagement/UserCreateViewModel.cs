using System.ComponentModel.DataAnnotations;
namespace Echeckdem.CustomFolder.UserManagement

{
    public class UserCreateViewModel
    {
        [Required]
        public string UserID { get; set; }
        [Required]
        public string UNAME { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string EmailID { get; set; }
        [Required]
        public int? UserLevel { get; set; }
        [Required]
        public string OID { get; set; }
    }
}
