using App.Application.Configuration.Queries;
using App.Domain.Users;

namespace App.Application.Users.GetUsersPhotos;

public class GetUsersPhotosQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUsersPhotosQuery, IDictionary<string, string>>
{
    public async Task<IDictionary<string, string>> Handle(GetUsersPhotosQuery query, CancellationToken cancellationToken)
    {
        return await userRepository.GetUsersPhotosAsync(query.UsersIds);
    }
}
