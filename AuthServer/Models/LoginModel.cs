using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AuthServer.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
