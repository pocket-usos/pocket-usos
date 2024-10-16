using App.Application.Configuration.Queries;

namespace App.Application.Users.GetUser;

public class GetUserQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUserQuery, User>
{
    public async Task<User> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        return await userRepository.GetByIdAsync(query.UserId);
    }
}
