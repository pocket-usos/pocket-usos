using App.Application.Configuration;
using App.Infrastructure.Configuration.DataAccess;
using App.Infrastructure.Integration.Client;
using App.Infrastructure.Integration.Requests;

namespace App.Infrastructure.Integration.Usos.Courses;

internal class UsosCoursesProvider(IUsosHttpClient client, IExecutionContextAccessor context, ICacheProvider cache) : ICoursesProvider
{
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
}
