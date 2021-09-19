using System.ComponentModel.DataAnnotations;

namespace CreditVillageBackend
{
    public class UserUpdateRequest
    {
        [Required]
        [Display(Name = "Full Name")]
        public string Full_Name { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Bio")]
        public string Bio { get; set; }

        [Required]
        [Display(Name = "Nationality")]
        public int NationalityId { get; set; }

        [Required]
        [Display(Name = "State")]
        public int StateId { get; set; }

        [Display(Name = "Phone Number")]
        public string Phone_Number { get; set; }
    }
}