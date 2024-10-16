using App.Application.Configuration.Queries;

namespace App.Application.Courses.GetCoursesForTerm;

public class GetCoursesForTermQueryHandler(ICourseRepository courseRepository) : IQueryHandler<GetCoursesForTermQuery, IEnumerable<Course>>
{
    public async Task<IEnumerable<Course>> Handle(GetCoursesForTermQuery query, CancellationToken cancellationToken)
    {
        return await courseRepository.GetMyCoursesForTerm(query.Term, query.WithSchedule);
    }
}
