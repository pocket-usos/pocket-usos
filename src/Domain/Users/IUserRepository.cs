namespace App.Domain.Users;

public interface IUserRepository
{
    Task<Profile> GetCurrentAsync();

    Task<User> GetByIdAsync(string id);

    Task<List<User>> GetMultipleAsync(string[] ids);
}
