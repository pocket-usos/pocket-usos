using App.Application.Contracts;
using App.Domain.Students;

namespace App.Application.Students.GetStudent;

public class GetStudentQuery(string studentId) : QueryBase<Student>
{
    public string StudentId { get; } = studentId;
}
