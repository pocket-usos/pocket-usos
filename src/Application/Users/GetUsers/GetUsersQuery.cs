using App.Application.Contracts;
using App.Domain.Users;

namespace App.Application.Users.GetUsers;

public class GetUsersQuery(string[] userIds) : QueryBase<User[]>
{
    public string[] UserIds { get; } = userIds;
}
