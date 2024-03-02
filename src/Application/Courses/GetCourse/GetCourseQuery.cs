using App.Application.Contracts;

namespace App.Application.Courses.GetCourse;

public class GetCourseQuery(string courseId, string courseUnitId) : QueryBase<Course>
{
    public string CourseId { get; set; } = courseId;

    public string CourseUnitId { get; set; } = courseUnitId;
}
