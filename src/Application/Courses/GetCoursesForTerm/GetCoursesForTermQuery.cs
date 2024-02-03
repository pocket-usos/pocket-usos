using App.Application.Contracts;
using App.Domain.Courses;

namespace App.Application.Courses.GetCoursesForTerm;

public class GetCoursesForTermQuery(string term) : QueryBase<IEnumerable<Course>>
{
    public string Term { get; set; } = term;
}
