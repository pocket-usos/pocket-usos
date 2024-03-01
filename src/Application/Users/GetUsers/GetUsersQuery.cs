using App.Application.Contracts;

namespace App.Application.Users.GetUsers;

public class GetUsersQuery(string[] userIds) : QueryBase<User[]>
{
    public string[] UserIds { get; } = userIds;
}
