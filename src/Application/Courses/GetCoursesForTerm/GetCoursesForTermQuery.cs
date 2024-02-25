using App.Application.Contracts;

namespace App.Application.Courses.GetCoursesForTerm;

public class GetCoursesForTermQuery(string term) : QueryBase<IEnumerable<CourseDto>>
{
    public string Term { get; set; } = term;
}
