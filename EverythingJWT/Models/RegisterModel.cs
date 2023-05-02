using System.ComponentModel.DataAnnotations;

namespace EverythingJWT.Models
{
    public class RegisterModel: LoginModel
    {

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
    }
}
