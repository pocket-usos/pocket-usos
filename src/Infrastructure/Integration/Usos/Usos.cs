using App.Application.Configuration;
using App.Domain.UserAccess.Authentication;
using App.Infrastructure.Configuration.DataAccess;
using App.Infrastructure.Integration.Client;
using App.Infrastructure.Integration.Requests;
using App.Infrastructure.Integration.Usos.Courses;
using App.Infrastructure.Integration.Usos.Grades;
using App.Infrastructure.Integration.Usos.Students;
using App.Infrastructure.Integration.Usos.Terms;
using App.Infrastructure.Integration.Usos.TimeTable;
using Microsoft.AspNetCore.WebUtilities;

namespace App.Infrastructure.Integration.Usos;

internal class Usos(IUsosHttpClient client, IExecutionContextAccessor context, ICacheProvider cache) : IAuthenticationService, IUsersProvider, IGradesProvider, ICoursesProvider, ITimeTableProvider, ITermsProvider
{
    public async Task<RequestToken> RetrieveRequestToken()
    {
        var request = Request.RequestToken("services/oauth/request_token")
            .WithQueryParameter("scopes", String.Join('|', Scope.AllValues));

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful() || response.Content is null)
        {
            throw response.ToException(context.Language);
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
            throw response.ToException(context.Language);
        }

        var formReader = new FormReader(response.Content.AsString());
        var form = formReader.ReadForm();

        return new AccessToken(form["oauth_token"]!, form["oauth_token_secret"]!);
    }

    public async Task<UserDto> GetUser(string? id = null)
    {


        var request = Request.Get("services/users/user")
            .WithQueryParameter("fields",
                "id|first_name|last_name|sex|student_status|email|phone_numbers|mobile_numbers|photo_urls|student_number|pesel|birth_date|citizenship|student_programmes|postal_addresses|library_card_id|titles|office_hours|course_editions_conducted");

        if (id is not null)
        {
            request.WithQueryParameter("user_id", id);
        }

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        return response.Content!.As<UserDto>();
    }

    public async Task<IDictionary<string, UserDto>> GetMultipleUsers(string[] ids)
    {
        var request = Request.Get("services/users/users")
            .WithQueryParameter("fields", "id|first_name|last_name|sex|student_status|email|phone_numbers|mobile_numbers|photo_urls|student_number|pesel|birth_date|citizenship|student_programmes|postal_addresses|library_card_id|titles|office_hours|course_editions_conducted")
            .WithQueryParameter("user_ids", String.Join('|', ids));

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
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
            throw response.ToException(context.Language);
        }

        return response.Content!.As<IDictionary<string, IDictionary<string, TermCourseDto>>>();
    }

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

    public async Task<CourseDto> GetCourse(string id)
    {
        var course = await cache.GetAsync<CourseDto>($"usos-course-{id}");

        if (course is not null)
        {
            return course;
        }

        var request = Request.Get("services/courses/course")
            .WithQueryParameter("course_id", id);

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        course = response.Content!.As<CourseDto>();

        await cache.SetAsync($"usos-course-{id}", course, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
        });

        return course;
    }

    public async Task<CourseEditionDto> GetCourseEdition(string courseId, string termId)
    {
        var courseEdition = await cache.GetAsync<CourseEditionDto>($"usos-course-{courseId}-edition-{termId}");

        if (courseEdition is not null)
        {
            return courseEdition;
        }

        var request = Request.Get("services/courses/course_edition")
            .WithQueryParameter("course_id", courseId)
            .WithQueryParameter("term_id", termId);

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        courseEdition = response.Content!.As<CourseEditionDto>();

        await cache.SetAsync($"usos-course-{courseId}-edition-{termId}", courseEdition, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
        });

        return courseEdition;
    }

    public async Task<string> GetCourseUnitTypeId(string id)
    {
        var courseUnitTypeId = await cache.GetAsync<string>($"usos-course-unit-type-{id}");

        if (courseUnitTypeId is not null)
        {
            return courseUnitTypeId;
        }

        var request = Request.Get("services/courses/unit")
            .WithQueryParameter("unit_id", id)
            .WithQueryParameter("fields", "classtype_id");

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        var classTypeId = response.Content!.As<CourseUnitTypeIdDto>();

        await cache.SetAsync($"usos-course-unit-type-{id}", classTypeId.ClasstypeId, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
        });

        return classTypeId.ClasstypeId;
    }

    public async Task<string> GetCourseUnitTermId(string id)
    {
        var courseUnitTermId = await cache.GetAsync<string>($"usos-course-unit-term-{id}");

        if (courseUnitTermId is not null)
        {
            return courseUnitTermId;
        }

        var request = Request.Get("services/courses/unit")
            .WithQueryParameter("unit_id", id)
            .WithQueryParameter("fields", "term_id");

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        var classTypeId = response.Content!.As<CourseUnitTermIdDto>();

        await cache.SetAsync($"usos-course-unit-term-{id}", classTypeId.TermId, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
        });

        return classTypeId.TermId;
    }

    public async Task<IDictionary<string, ClassTypeDto>> GetClassTypes()
    {
        var classTypes = await cache.GetAsync<IDictionary<string, ClassTypeDto>>("usos-class-types");

        if (classTypes is not null)
        {
            return classTypes;
        }

        var request = Request.Get("services/courses/classtypes_index");

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        classTypes = response.Content!.As<IDictionary<string, ClassTypeDto>>();

        await cache.SetAsync("usos-class-types", classTypes, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
        });

        return classTypes;
    }

    public async Task<UserCoursesDto> GetUserCourses()
    {
        var userCourses = await cache.GetAsync<UserCoursesDto>($"usos-user-courses-{context.SessionId}");

        if (userCourses is not null)
        {
            return userCourses;
        }

        var request = Request.Get("services/courses/user")
            .WithQueryParameter("fields", "course_editions|terms");

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        userCourses = response.Content!.As<UserCoursesDto>();

        await cache.SetAsync($"usos-user-courses-{context.SessionId}", userCourses, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
        });

        return userCourses;
    }

    public async Task<CourseScheduleItemDto[]> GetCourseSchedule(string courseUnitId, int groupNumber)
    {
        var courseSchedule = await cache.GetAsync<CourseScheduleItemDto[]>($"usos-course-schedule-{courseUnitId}-{groupNumber}");

        if (courseSchedule is not null)
        {
            return courseSchedule;
        }

        var request = Request.Get("services/tt/classgroup_dates2")
            .WithQueryParameter("unit_id", courseUnitId)
            .WithQueryParameter("group_number", groupNumber);

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        courseSchedule = response.Content!.As<CourseScheduleItemDto[]>();

        await cache.SetAsync($"usos-course-schedule-{courseUnitId}-{groupNumber}", courseSchedule, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
        });

        return courseSchedule;
    }

    public async Task<IEnumerable<TimeTableItemDto>> GetUserTimeTable(DateOnly start, int days)
    {
        var timeTable = await cache.GetAsync<IEnumerable<TimeTableItemDto>>($"usos-user-{context.SessionId}-timetable-{start.ToString("yyyy-MM-dd")}-{days}");

        if (timeTable is not null)
        {
            return timeTable;
        }

        var request = Request.Get("services/tt/user")
            .WithQueryParameter("fields",
                "start_time|end_time|name|course_id|course_name|classtype_id|classtype_name|lecturer_ids|group_number|building_name|building_id|room_number|room_id|unit_id|classtype_id|cgwm_id|frequency")
            .WithQueryParameter("start", start.ToString("yyyy-MM-dd"))
            .WithQueryParameter("days", days.ToString());

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        timeTable = response.Content!.As<IEnumerable<TimeTableItemDto>>().ToList();

        await cache.SetAsync($"usos-user-{context.SessionId}-timetable-{start.ToString("yyyy-MM-dd")}-{days}", timeTable, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
        });

        return timeTable;
    }

    public async Task<IEnumerable<TimeTableItemDto>> GetStaffTimeTable(string userId, DateOnly start, int days)
    {
        var timeTable = await cache.GetAsync<IEnumerable<TimeTableItemDto>>($"usos-staff-{userId}-timetable-{start.ToString("yyyy-MM-dd")}-{days}");

        if (timeTable is not null)
        {
            return timeTable;
        }

        var request = Request.Get("services/tt/staff")
            .WithQueryParameter("user_id", userId)
            .WithQueryParameter("fields",
                "start_time|end_time|name|course_id|course_name|classtype_id|classtype_name|lecturer_ids|group_number|building_name|building_id|room_number|room_id|unit_id|classtype_id|cgwm_id|frequency")
            .WithQueryParameter("start", start.ToString("yyyy-MM-dd"))
            .WithQueryParameter("days", days.ToString());

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        timeTable = response.Content!.As<IEnumerable<TimeTableItemDto>>().ToList();

        await cache.SetAsync($"usos-staff-{userId}-timetable-{start.ToString("yyyy-MM-dd")}-{days}", timeTable, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
        });

        return timeTable;
    }

    public async Task<TermsDto> GetTerms()
    {
        var terms = await cache.GetAsync<TermsDto>($"usos-user-terms-{context.SessionId}");

        if (terms is not null)
        {
            return terms;
        }

        var request = Request.Get("services/courses/user")
            .WithQueryParameter("fields", "terms");

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        terms = response.Content!.As<TermsDto>();

        await cache.SetAsync($"usos-user-terms-{context.SessionId}", terms, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
        });

        return terms;
    }
}
