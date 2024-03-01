using App.Application.Configuration.Queries;

namespace App.Application.Courses.GetCourse;

public class GetCourseQueryHandler(ICourseRepository courseRepository) : IQueryHandler<GetCourseQuery, Course>
{
    public async Task<Course> Handle(GetCourseQuery query, CancellationToken cancellationToken)
    {
        return await courseRepository.GetCourse(query.CourseId, query.CourseUnitId);
    }
}
