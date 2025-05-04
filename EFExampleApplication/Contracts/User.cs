namespace EFExampleApplication.Contracts;

public record UserListVm(int Id, string Login);
public record ListOfUsers(IReadOnlySet<UserListVm> Users);

public record UserVm(int Id, string Login);

public record CreateUserDto(string Login, string Password);

public record UpdateUserDto(string Login);
