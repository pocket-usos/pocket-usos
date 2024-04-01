using App.Domain.UserAccess.Authentication;

namespace App.Application.Grades;

public interface IGradesRepository
{
    Task<TermGrades> GetGradesForTerm(string term);

    Task<SessionGrade> GetGrade(AuthenticationSessionId sessionId, string examId, int sessionNumber);

    Task<GradesDistributionItem[]> GetGradesDistribution(string examId);
}
