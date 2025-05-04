namespace EFExampleApplication.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(int id) : base($"User with id {id} not found") { }
    public UserNotFoundException(string login) : base($"User with login {login} not found") { }
}
