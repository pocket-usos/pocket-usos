using App.Application.Configuration;
using App.Infrastructure.Integration.Client;
using App.Infrastructure.Integration.Requests;

namespace App.Infrastructure.Integration.Usos.Courses;

internal class UsosCoursesProvider(IUsosHttpClient client, IAuthorizedRequestFactory requestFactory, IExecutionContextAccessor context) : ICoursesProvider
{
    public async Task<IDictionary<string, ClassTypeDto>> GetClassTypes()
    {
        var request = await requestFactory.CreateGetRequestAsync("services/courses/classtypes_index");

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        return response.Content!.As<IDictionary<string, ClassTypeDto>>();
    }

    public async Task<CourseDto> GetCourse(string id)
    {
        var request = await requestFactory.CreateGetRequestAsync("services/courses/course",
            r => r.WithQueryParameter("course_id", id));

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        return response.Content!.As<CourseDto>();
    }

    public async Task<CourseEditionDto> GetCourseEdition(string courseId, string termId)
    {
        var request = await requestFactory.CreateGetRequestAsync(
            "services/courses/course_edition",
            r => r.WithQueryParameter("course_id", courseId)
                  .WithQueryParameter("term_id", termId));

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        return response.Content!.As<CourseEditionDto>();
    }

    public async Task<CourseScheduleItemDto[]> GetCourseSchedule(string courseUnitId, int groupNumber)
    {
        var request = await requestFactory.CreateGetRequestAsync("services/tt/classgroup_dates2",
            r => r.WithQueryParameter("unit_id", courseUnitId)
                  .WithQueryParameter("group_number", groupNumber));

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        return response.Content!.As<CourseScheduleItemDto[]>();
    }

    public async Task<string> GetCourseUnitTermId(string id)
    {
        var request = await requestFactory.CreateGetRequestAsync("services/courses/unit",
            r => r.WithQueryParameter("unit_id", id)
                  .WithQueryParameter("fields", "term_id"));

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        var classTypeId = response.Content!.As<CourseUnitTermIdDto>();

        return classTypeId.TermId;
    }

    public async Task<string> GetCourseUnitTypeId(string id)
    {
        var request = await requestFactory.CreateGetRequestAsync("services/courses/unit",
            r => r.WithQueryParameter("unit_id", id)
                  .WithQueryParameter("fields", "classtype_id"));

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        var classTypeId = response.Content!.As<CourseUnitTypeIdDto>();

        return classTypeId.ClasstypeId;
    }

    public async Task<UserCoursesDto> GetUserCourses()
    {
        var request = await requestFactory.CreateGetRequestAsync("services/courses/user",
            r => r.WithQueryParameter("fields", "course_editions|terms"));

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        return response.Content!.As<UserCoursesDto>();
    }
}
