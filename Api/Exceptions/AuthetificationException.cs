namespace Api.Exceptions
{
    public class AuthetificationException : Exception
    {
        public AuthetificationException() : base("You are not authentificated!")
        {
        }
    }
}
