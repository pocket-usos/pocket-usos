namespace App.Infrastructure.Integration.Usos.Students;

public interface IUsersProvider
{
    Task<UserDto> GetUser(string? id = null);

    Task<IDictionary<string, UserDto>> GetMultipleUsers(string[] ids);
}
