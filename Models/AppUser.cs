using System;
using Microsoft.AspNetCore.Identity;

namespace CreditVillageBackend
{
    public class AppUser : IdentityUser
    {
        public string Full_Name { get; set; }

        public string Address { get; set; }

        public string Bio { get; set; }

        public string Image_Url { get; set; }

        public DateTime Joined_Date { get; set; }

        public int NationalityId { get; set; }

        public int StateId { get; set; }

        public Nationality Nationality { get; set; }

        public State State { get; set; }
    }
}