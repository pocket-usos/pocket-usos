using App.Application.Contracts;

namespace App.Application.Users.GetUsersPhotos;

public class GetUsersPhotosQuery(string[] usersIds) : QueryBase<IDictionary<string, string>>
{
    public string[] UsersIds { get; set; } = usersIds;
}
