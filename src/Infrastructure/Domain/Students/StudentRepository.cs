using App.Domain.Students;
using App.Infrastructure.Integration.Usos.Students;

namespace App.Infrastructure.Domain.Students;

public class StudentRepository(IStudentsProvider studentsProvider) : IStudentRepository
{
    public async Task<Profile> GetCurrentAsync()
    {
        var studentDto = await studentsProvider.GetStudent();

        return studentDto.ToProfile();
    }

    public async Task<Student> GetByIdAsync(string id)
    {
        var studentDto = await studentsProvider.GetStudent(id);

        return studentDto.ToStudent();
    }
}
