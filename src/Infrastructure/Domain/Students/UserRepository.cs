using App.Domain.Users;
using App.Infrastructure.Integration.Usos.Students;
using Microsoft.Extensions.Caching.Memory;

namespace App.Infrastructure.Domain.Students;

public class UserRepository(IUsersProvider usersProvider, IMemoryCache cache) : IUserRepository
{
    public async Task<Profile> GetCurrentAsync()
    {
        var userDto = await usersProvider.GetUser();

        return userDto.ToProfile();
    }

    public async Task<User> GetByIdAsync(string id)
    {
        if (!cache.TryGetValue($"user-{id}", out User? user))
        {
            var userDto = await usersProvider.GetUser(id);
            user = userDto.ToUser();

            cache.Set($"user-{id}", user);

            return user;
        }

        return user!;
    }

    public async Task<List<User>> GetMultipleAsync(string[] ids)
    {
        var users = new List<User>();
        var userIdsToFetch = new List<string>();

        foreach (var id in ids)
        {
            if (!cache.TryGetValue($"user={id}", out User? user))
            {
                userIdsToFetch.Add(id);
            }

            if (user is not null) users.Add(user);
        }

        var usersDictionary = await usersProvider.GetMultipleUsers(userIdsToFetch.ToArray());
        var fetchedUsers = usersDictionary.Values.Select(userDto => userDto.ToUser()).ToList();

        foreach (var user in fetchedUsers)
        {
            cache.Set($"user={user.Id}", user);
        }

        users.AddRange(fetchedUsers);

        return users;
    }
}
