namespace CreditVillageBackend
{
    public class UserUpdateRequest
    {
        public string Full_Name { get; set; }

        public string Address { get; set; }

        public string Bio { get; set; }

        public int NationalityId { get; set; }

        public int StateId { get; set; }

        public string Phone_Number { get; set; }
    }
}