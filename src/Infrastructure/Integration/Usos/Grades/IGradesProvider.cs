namespace App.Infrastructure.Integration.Usos.Grades;

public interface IGradesProvider
{
    Task<IDictionary<string, IDictionary<string, TermCourseDto>>> GetGradesForTerm(string term);

    Task<GradeDto> GetGrade(Guid sessionId, string examId, int sessionNumber);

    Task<GradesDistributionDto> GetExamGradesDistribution(string examId);
}
