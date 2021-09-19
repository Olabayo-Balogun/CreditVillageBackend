using System;

namespace CreditVillageBackend
{
    public class UserLoginMapping : ResponseMapping<UserLoginConfirm>
    {

    }

    public class UserLoginConfirm
    {
        public string Token { get; set; }

        public DateTime Expiration { get; set; }
    }
}