namespace Auth.Core.Exceptions;

public class LoginException : Exception
{
    public LoginException(string message) : base("Login failed. " + message)
    {
    }
}