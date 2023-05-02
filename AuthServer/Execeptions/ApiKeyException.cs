namespace AuthServer.Execeptions
{
    public class ApiKeyException: ApplicationException
    {
        public ApiKeyException(string message = "Invalid ApiKey"): base(message, new ArgumentNullException())
        {
        }
    }
}
