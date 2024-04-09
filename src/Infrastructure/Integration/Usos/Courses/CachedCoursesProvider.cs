using App.Application.Configuration;
using App.Infrastructure.Configuration.DataAccess;

namespace App.Infrastructure.Integration.Usos.Courses;

internal class CachedCoursesProvider(ICoursesProvider coursesProvider, ICacheProvider cache, IExecutionContextAccessor context) : ICoursesProvider
{
    public async Task<IDictionary<string, ClassTypeDto>> GetClassTypes()
    {
        var classTypes = await cache.GetAsync<IDictionary<string, ClassTypeDto>>("usos-class-types");

        if (classTypes is not null)
        {
            return classTypes;
        }

        classTypes = await coursesProvider.GetClassTypes();

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

        course = await coursesProvider.GetCourse(id);

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

        courseEdition = await coursesProvider.GetCourseEdition(courseId, termId);

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

        courseSchedule = await coursesProvider.GetCourseSchedule(courseUnitId, groupNumber);

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

        courseUnitTermId = await coursesProvider.GetCourseUnitTermId(id);

        await cache.SetAsync($"usos-course-unit-term-{id}", courseUnitTermId, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
        });

        return courseUnitTermId;
    }

    public async Task<string> GetCourseUnitTypeId(string id)
    {
        var courseUnitTypeId = await cache.GetAsync<string>($"usos-course-unit-type-{id}");

        if (courseUnitTypeId is not null)
        {
            return courseUnitTypeId;
        }

        courseUnitTypeId = await coursesProvider.GetCourseUnitTypeId(id);

        await cache.SetAsync($"usos-course-unit-type-{id}", courseUnitTypeId, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
        });

        return courseUnitTypeId;
    }

    public async Task<UserCoursesDto> GetUserCourses()
    {
        var userCourses = await cache.GetAsync<UserCoursesDto>($"usos-user-courses-{context.SessionId}");

        if (userCourses is not null)
        {
            return userCourses;
        }

        userCourses = await coursesProvider.GetUserCourses();

        await cache.SetAsync($"usos-user-courses-{context.SessionId}", userCourses, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
        });

        return userCourses;
    }
}
