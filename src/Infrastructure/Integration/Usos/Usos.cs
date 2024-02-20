using App.Domain.UserAccess.Authentication;
using App.Infrastructure.Integration.Client;
using App.Infrastructure.Integration.Exceptions;
using App.Infrastructure.Integration.Requests;
using App.Infrastructure.Integration.Usos.Courses;
using App.Infrastructure.Integration.Usos.Grades;
using App.Infrastructure.Integration.Usos.Students;
using App.Infrastructure.Integration.Usos.Terms;
using App.Infrastructure.Integration.Usos.TimeTable;
using Microsoft.AspNetCore.WebUtilities;

namespace App.Infrastructure.Integration.Usos;

internal class Usos(IUsosHttpClient client) : IAuthenticationService, IUsersProvider, IGradesProvider, ICoursesProvider, ITimeTableProvider, ITermsProvider
{
    public async Task<RequestToken> RetrieveRequestToken()
    {
        var request = Request.RequestToken("services/oauth/request_token")
            .WithQueryParameter("scopes", String.Join('|', Scope.AllValues));

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful() || response.Content is null)
        {
            throw new UsosIntegrationException(response.Error!.Message);
        }

        var formReader = new FormReader(response.Content.AsString());
        var form = formReader.ReadForm();

        return new RequestToken(form["oauth_token"]!, form["oauth_token_secret"]!);
    }

    public async Task<AccessToken> RetrieveAccessToken(string token, string tokenSecret, string verifier)
    {
        var request = Request.AccessToken("services/oauth/access_token", token, tokenSecret, verifier);

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful() || response.Content is null)
        {
            throw new UsosIntegrationException(response.Error!.Message);
        }

        var formReader = new FormReader(response.Content.AsString());
        var form = formReader.ReadForm();

        return new AccessToken(form["oauth_token"]!, form["oauth_token_secret"]!);
    }

    public async Task<UserDto> GetUser(string? id = null)
    {
        var request = Request.Get("services/users/user")
            .WithQueryParameter("fields",
                "id|first_name|last_name|sex|student_status|email|phone_numbers|mobile_numbers|photo_urls|student_number|pesel|birth_date|citizenship|student_programmes|postal_addresses|library_card_id");

        if (id is not null)
        {
            request.WithQueryParameter("user_id", id);
        }

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new UsosIntegrationException(response.Error!.Message);
        }

        return response.Content!.As<UserDto>();
    }

    public async Task<IDictionary<string, UserDto>> GetMultipleUsers(string[] ids)
    {
        var request = Request.Get("services/users/users")
            .WithQueryParameter("fields", "id|first_name|last_name|sex|student_status|email|phone_numbers|mobile_numbers|photo_urls|student_number|pesel|birth_date|citizenship|student_programmes|postal_addresses|library_card_id")
            .WithQueryParameter("user_ids", String.Join('|', ids));

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new UsosIntegrationException(response.Error!.Message);
        }

        return response.Content!.As<IDictionary<string, UserDto>>();
    }

    public async Task<IDictionary<string, IDictionary<string, TermCourseDto>>> GetGradesForTerm(string term)
    {
        var request = Request.Get("services/grades/terms2")
            .WithQueryParameter("term_ids", term)
            .WithQueryParameter("fields", "value_symbol|passes|value_description|exam_id|exam_session_number|counts_into_average|comment|grade_type_id|date_modified|date_acquisition|modification_author");

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new UsosIntegrationException(response.Error!.Message);
        }

        return response.Content!.As<IDictionary<string, IDictionary<string, TermCourseDto>>>();
    }

    public async Task<GradesDistributionDto> GetExamGradesDistribution(string examId)
    {
        var request = Request.Get("services/examrep/exam")
            .WithQueryParameter("id", examId)
            .WithQueryParameter("fields", "grades_distribution");

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new UsosIntegrationException(response.Error!.Message);
        }

        return response.Content!.As<GradesDistributionDto>();
    }

    public async Task<CourseDto> GetCourse(string id)
    {
        var request = Request.Get("services/courses/course")
            .WithQueryParameter("course_id", id);

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new UsosIntegrationException(response.Error!.Message);
        }

        return response.Content!.As<CourseDto>();
    }

    public async Task<CourseUnitDto> GetCourseUnit(string id)
    {
        var request = Request.Get("services/courses/unit")
            .WithQueryParameter("unit_id", id)
            .WithQueryParameter("fields", "id|classtype_id");

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new UsosIntegrationException(response.Error!.Message);
        }

        return response.Content!.As<CourseUnitDto>();
    }

    public async Task<IDictionary<string, ClassTypeDto>> GetClassTypes()
    {
        var request = Request.Get("services/courses/classtypes_index");

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new UsosIntegrationException(response.Error!.Message);
        }

        return response.Content!.As<IDictionary<string, ClassTypeDto>>();
    }

    public async Task<UserCoursesDto> GetUserCourses()
    {
        var request = Request.Get("services/courses/user")
            .WithQueryParameter("fields", "course_editions|terms");

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new UsosIntegrationException(response.Error!.Message);
        }

        return response.Content!.As<UserCoursesDto>();
    }

    public async Task<IEnumerable<TimeTableItemDto>> GetUserTimeTable(DateOnly? start, int? days)
    {
        var request = Request.Get("services/tt/user")
            .WithQueryParameter("fields",
                "start_time|end_time|name|course_id|course_name|classtype_id|classtype_name|lecturer_ids|group_number|building_name|building_id|room_number|room_id|unit_id|classtype_id|cgwm_id|frequency");

        if (start is not null) request.WithQueryParameter("start", start.Value.ToString("yyyy-MM-dd"));
        if (days is not null) request.WithQueryParameter("days", days.Value.ToString());

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new UsosIntegrationException(response.Error!.Message);
        }

        return response.Content!.As<IEnumerable<TimeTableItemDto>>();
    }

    public async Task<TermsDto> GetTerms()
    {
        var request = Request.Get("services/courses/user")
            .WithQueryParameter("fields", "terms");

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new UsosIntegrationException(response.Error!.Message);
        }

        return response.Content!.As<TermsDto>();
    }
}
