using App.Application.Contracts;
using App.Domain.Grades;

namespace App.Application.Grades.GetTermGrades;

public class GetTermGradesQuery(string? term) : QueryBase<TermGrades>
{
    public string? Term { get; } = term;
}
