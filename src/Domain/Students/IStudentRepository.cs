namespace App.Domain.Students;

public interface IStudentRepository
{
    Task<Profile> GetCurrentAsync();

    Task<Student> GetByIdAsync(string id);
}
