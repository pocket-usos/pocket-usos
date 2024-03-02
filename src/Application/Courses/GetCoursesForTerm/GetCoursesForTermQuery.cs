using App.Application.Contracts;

namespace App.Application.Courses.GetCoursesForTerm;

public class GetCoursesForTermQuery(string term, bool withSchedule) : QueryBase<IEnumerable<Course>>
{
    public string Term { get; set; } = term;

    public bool WithSchedule { get; set; } = withSchedule;
}
