using Microsoft.AspNetCore.Identity;

namespace EverythingJWT.Data
{
    public class ApiUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
