using App.Domain.Courses;

namespace App.Application.Courses;

public interface ICourseRepository
{
    public Task<Course> GetCourse(string id);

    public Task<CourseUnit> GetCourseUnit(string id);

    public Task<List<CourseDto>> GetMyCoursesForTerm(string termId, bool withSchedule = false);
}
