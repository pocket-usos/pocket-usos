using App.Application.Configuration.Queries;
using App.Domain.Students;

namespace App.Application.Students.GetMyProfile;

public class GetMyProfileQueryHandler(IStudentRepository studentRepository) : IQueryHandler<GetMyProfileQuery, Profile>
{
    public async Task<Profile> Handle(GetMyProfileQuery query, CancellationToken cancellationToken)
    {
        return await studentRepository.GetCurrentAsync();
    }
}
