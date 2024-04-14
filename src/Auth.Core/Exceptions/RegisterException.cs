namespace Auth.Core.Exceptions;

public class RegisterException : Exception
{
    public RegisterException(string message) : base("Register failed. " + message)
    {
    }
}