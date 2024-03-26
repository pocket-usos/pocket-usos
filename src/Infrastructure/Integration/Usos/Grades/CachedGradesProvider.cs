using App.Application.Configuration;
using App.Infrastructure.Configuration.DataAccess;

namespace App.Infrastructure.Integration.Usos.Grades;

internal class CachedGradesProvider(IGradesProvider gradesProvider, ICacheProvider cache, IExecutionContextAccessor context) : IGradesProvider
{
    public async Task<GradesDistributionDto> GetExamGradesDistribution(string examId)
    {
        var gradesDistribution = await cache.GetAsync<GradesDistributionDto>($"usos-grades-distribution-{examId}");

        if (gradesDistribution is not null)
        {
            return gradesDistribution;
        }

        gradesDistribution = await gradesProvider.GetExamGradesDistribution(examId);

        await cache.SetAsync($"usos-grades-distribution-{examId}", gradesDistribution, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
        });

        return gradesDistribution;
    }

    public async Task<IDictionary<string, IDictionary<string, TermCourseDto>>> GetGradesForTerm(string term)
    {
        var grades = await cache.GetAsync<IDictionary<string, IDictionary<string, TermCourseDto>>>($"usos-user-{context.SessionId}-grades-{term}");

        if (grades is not null)
        {
            return grades;
        }

        grades = await gradesProvider.GetGradesForTerm(term);

        await cache.SetAsync($"usos-user-{context.SessionId}-grades-{term}", grades, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
        });

        return grades;
    }
}
