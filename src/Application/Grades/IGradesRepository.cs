namespace App.Application.Grades;

public interface IGradesRepository
{
    Task<TermGrades> GetGradesForTerm(string term);
}
