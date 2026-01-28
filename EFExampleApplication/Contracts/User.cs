namespace EFExampleApplication.Contracts;

public record UserListVm(UserId Id, string Login);
public record ListOfUsers(IReadOnlyCollection<UserListVm> Users);

public record UserVm(UserId Id, string Login);

public record CreateUserDto(string Login, string Password);

public record UpdateUserDto(string Login);
