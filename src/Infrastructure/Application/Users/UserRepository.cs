using App.Application.Configuration;
using App.Application.Users;
using App.Infrastructure.Configuration.DataAccess;
using App.Infrastructure.Integration.Usos.Students;
namespace App.Infrastructure.Application.Users;

public class UserRepository(IUsersProvider usersProvider, IExecutionContextAccessor context, ICacheProvider cache) : IUserRepository
{
    public async Task<Profile> GetCurrentAsync()
    {
        var profile = await cache.GetAsync<Profile>($"profile-{context.SessionId.ToString()}");

        if (profile is null)
        {
            var userDto = await usersProvider.GetUser();
            profile = userDto.ToProfile();

            await cache.SetAsync($"profile-{context.SessionId.ToString()}", profile, options =>
            {
                options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
            });
        }

        return profile;
    }

    public async Task<User> GetByIdAsync(string id)
    {
        var user = await cache.GetAsync<User>($"user-{id}");

        if (user is null)
        {
            var userDto = await usersProvider.GetUser(id);
            user = userDto.ToUser(context.Language);

            await cache.SetAsync($"user-{id}", user, options =>
            {
                options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
            });

            return user;
        }

        return user;
    }

    public async Task<List<User>> GetMultipleAsync(string[] ids)
    {
        var users = new List<User>();
        var userIdsToFetch = new List<string>();

        foreach (var id in ids)
        {
            var user = await cache.GetAsync<User>($"user-{id}");

            if (user is null)
            {
                userIdsToFetch.Add(id);
            }
            else
            {
                users.Add(user);
            }
        }

        if (userIdsToFetch.Count == 0)
        {
            return users;
        }

        var usersDictionary = await usersProvider.GetMultipleUsers(userIdsToFetch.ToArray());
        var fetchedUsers = usersDictionary.Values.Select(userDto => userDto.ToUser(context.Language)).ToList();

        foreach (var user in fetchedUsers)
        {
            await cache.SetAsync($"user-{user.Id}", user, options =>
            {
                options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
            });
        }

        users.AddRange(fetchedUsers);

        return users;
    }

    public async Task<IDictionary<string, string>> GetUsersPhotosAsync(string[] usersIds)
    {
        var usersIdsChunks = usersIds.Select((id, index) => new { Value = id, Index = index })
            .GroupBy(x => x.Index / 50)
            .Select(x => x.Select(y => y.Value).ToArray())
            .ToArray();

        var usersPhotos = new Dictionary<string, string>();

        foreach (var userIdsChunk in usersIdsChunks)
        {
            var users = await GetMultipleAsync(userIdsChunk);
            foreach (var user in users)
            {
                usersPhotos[user.Id] = user.PhotoUrl;
            }
        }

        return usersPhotos;
    }
}
