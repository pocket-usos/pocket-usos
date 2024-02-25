namespace App.Infrastructure.Integration.Usos.Courses;

public interface ICoursesProvider
{
    public Task<CourseDto> GetCourse(string id);

    public Task<CourseUnitDto> GetCourseUnit(string id);

    public Task<IDictionary<string, ClassTypeDto>> GetClassTypes();

    public Task<UserCoursesDto> GetUserCourses();

    public Task<CourseScheduleItemDto[]> GetCourseSchedule(string courseUnitId, int groupNumber);
}
