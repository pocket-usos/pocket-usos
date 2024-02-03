using App.Application.Configuration.Queries;
using App.Domain.Students;

namespace App.Application.Students.GetStudent;

public class GetStudentQueryHandler(IStudentRepository studentRepository) : IQueryHandler<GetStudentQuery, Student>
{
    public async Task<Student> Handle(GetStudentQuery query, CancellationToken cancellationToken)
    {
        return await studentRepository.GetByIdAsync(query.StudentId);
    }
}
