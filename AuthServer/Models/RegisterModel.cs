using Microsoft.Build.Framework;

namespace AuthServer.Models
{
    public class RegisterModel: LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime DoB { get; set; }

    }
}
