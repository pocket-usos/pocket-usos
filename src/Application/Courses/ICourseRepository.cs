namespace App.Application.Courses;

public interface ICourseRepository
{
    public Task<Course> GetCourse(string courseId, string courseUnitId);

    public Task<List<Course>> GetMyCoursesForTerm(string termId, bool withSchedule = false);
}
