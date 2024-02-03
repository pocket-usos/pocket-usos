namespace App.Infrastructure.Integration.Usos.Students;

public interface IStudentsProvider
{
    Task<StudentDto> GetStudent(string? id = null);
}
