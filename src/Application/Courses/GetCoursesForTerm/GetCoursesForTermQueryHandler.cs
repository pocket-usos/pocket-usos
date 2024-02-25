using App.Application.Configuration.Queries;

namespace App.Application.Courses.GetCoursesForTerm;

public class GetCoursesForTermQueryHandler(ICourseRepository courseRepository) : IQueryHandler<GetCoursesForTermQuery, IEnumerable<CourseDto>>
{
    public async Task<IEnumerable<CourseDto>> Handle(GetCoursesForTermQuery query, CancellationToken cancellationToken)
    {
        return await courseRepository.GetMyCoursesForTerm(query.Term);
    }
}
