namespace App.Domain.Courses;

public interface ICourseRepository
{
    public Task<Course> GetCourse(string id);

    public Task<CourseUnit> GetCourseUnit(string id);

    public Task<List<Course>> GetMyCoursesForTerm(string termId);
}
