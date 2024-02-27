using App.Application.Configuration.Queries;
using App.Domain.Users;

namespace App.Application.Users.GetUsers;

public class GetUsersQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUsersQuery, User[]>
{
    public async Task<User[]> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetMultipleAsync(query.UserIds);

        return users.ToArray();
    }
}
