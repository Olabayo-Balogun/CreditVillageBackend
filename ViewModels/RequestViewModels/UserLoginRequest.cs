using System.ComponentModel.DataAnnotations;

namespace CreditVillageBackend
{
    public class UserLoginRequest
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}