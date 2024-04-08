using App.Application.Configuration;
using App.Infrastructure.Configuration.DataAccess;
using App.Infrastructure.Integration.Client;
using App.Infrastructure.Integration.Requests;

namespace App.Infrastructure.Integration.Usos.Grades;

internal class UsosGradesProvider(IUsosHttpClient client, IAuthorizedRequestFactory requestFactory, IExecutionContextAccessor context) : IGradesProvider
{
    public async Task<GradesDistributionDto> GetExamGradesDistribution(string examId)
    {
        var request = await requestFactory.CreateGetRequestAsync("services/examrep/exam",
            r => r.WithQueryParameter("id", examId)
                  .WithQueryParameter("fields", "grades_distribution"));

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        return response.Content!.As<GradesDistributionDto>();
    }

    public async Task<IDictionary<string, IDictionary<string, TermCourseDto>>> GetGradesForTerm(string term)
    {
        var request = await requestFactory.CreateGetRequestAsync("services/grades/terms2",
            r => r.WithQueryParameter("term_ids", term)
                  .WithQueryParameter("fields", "value_symbol|passes|value_description|exam_id|exam_session_number|counts_into_average|comment|grade_type_id|date_modified|date_acquisition|modification_author"));

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        return response.Content!.As<IDictionary<string, IDictionary<string, TermCourseDto>>>();
    }
}
