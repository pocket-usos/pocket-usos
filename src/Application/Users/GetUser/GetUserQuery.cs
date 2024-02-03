using App.Application.Contracts;
using App.Domain.Users;

namespace App.Application.Users.GetUser;

public class GetUserQuery(string studentId) : QueryBase<User>
{
    public string StudentId { get; } = studentId;
}
