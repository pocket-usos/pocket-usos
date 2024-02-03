using App.Application.Configuration.Queries;
using App.Domain.Users;

namespace App.Application.Users.GetMyProfile;

public class GetMyProfileQueryHandler(IUserRepository userRepository) : IQueryHandler<GetMyProfileQuery, Profile>
{
    public async Task<Profile> Handle(GetMyProfileQuery query, CancellationToken cancellationToken)
    {
        return await userRepository.GetCurrentAsync();
    }
}
