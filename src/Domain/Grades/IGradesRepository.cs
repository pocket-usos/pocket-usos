namespace App.Domain.Grades;

public interface IGradesRepository
{
    Task<TermGrades> GetGradesForTerm(string term);
}
