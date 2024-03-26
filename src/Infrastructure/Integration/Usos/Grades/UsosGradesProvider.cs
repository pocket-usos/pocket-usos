using App.Application.Configuration;
using App.Infrastructure.Configuration.DataAccess;
using App.Infrastructure.Integration.Client;
using App.Infrastructure.Integration.Requests;

namespace App.Infrastructure.Integration.Usos.Grades;

internal class UsosGradesProvider(IUsosHttpClient client, IExecutionContextAccessor context, ICacheProvider cache) : IGradesProvider
{
    public async Task<GradesDistributionDto> GetExamGradesDistribution(string examId)
    {
        var gradesDistribution = await cache.GetAsync<GradesDistributionDto>($"usos-grades-distribution-{examId}");

        if (gradesDistribution is not null)
        {
            return gradesDistribution;
        }

        var request = Request.Get("services/examrep/exam")
            .WithQueryParameter("id", examId)
            .WithQueryParameter("fields", "grades_distribution");

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        gradesDistribution = response.Content!.As<GradesDistributionDto>();

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

        var request = Request.Get("services/grades/terms2")
            .WithQueryParameter("term_ids", term)
            .WithQueryParameter("fields", "value_symbol|passes|value_description|exam_id|exam_session_number|counts_into_average|comment|grade_type_id|date_modified|date_acquisition|modification_author");

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        grades = response.Content!.As<IDictionary<string, IDictionary<string, TermCourseDto>>>();

        await cache.SetAsync($"usos-user-{context.SessionId}-grades-{term}", grades, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
        });

        return grades;
    }
}
