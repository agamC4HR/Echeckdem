using System.ComponentModel.DataAnnotations;

namespace Echeckdem.CustomFolder
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User ID")]
        public string userID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}


