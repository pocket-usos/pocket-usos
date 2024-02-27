using App.Application.Contracts;
using App.Domain.Users;

namespace App.Application.Users.GetUser;

public class GetUserQuery(string userId) : QueryBase<User>
{
    public string UserId { get; } = userId;
}
