using System.ComponentModel.DataAnnotations;

namespace CreditVillageBackend
{
    public class UserResendCodeRequest
    {
        [Required]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}