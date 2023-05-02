using Microsoft.AspNetCore.Identity;

namespace AuthServer.Data
{
    public class AccountUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DoB { get; set; }

    }
}
