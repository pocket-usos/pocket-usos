using Polly;

namespace App.Infrastructure.Integration.Usos.Grades;

public interface IGradesProvider
{
    Task<IDictionary<string, IDictionary<string, TermCourseDto>>> GetGradesForTerm(string term);

    Task<GradesDistributionDto> GetExamGradesDistribution(string examId);
}
