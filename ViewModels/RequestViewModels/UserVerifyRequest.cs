using System.ComponentModel.DataAnnotations;

namespace CreditVillageBackend
{
    public class UserVerifyRequest
    {
        [Required]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required]
        [Display(Name = "Token")]
        public string Token { get; set; }
    }
}