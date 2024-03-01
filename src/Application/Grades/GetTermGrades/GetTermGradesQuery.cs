using App.Application.Contracts;

namespace App.Application.Grades.GetTermGrades;

public class GetTermGradesQuery(string? term) : QueryBase<TermGrades>
{
    public string? Term { get; } = term;
}
