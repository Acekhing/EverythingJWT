namespace AuthServer.Models
{
    public class RegistrationResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string EmailConfirmationCode { get; set; } = string.Empty;
    }
}
