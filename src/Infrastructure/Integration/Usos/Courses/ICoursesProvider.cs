namespace App.Infrastructure.Integration.Usos.Courses;

public interface ICoursesProvider
{
    public Task<CourseDto> GetCourse(string id);

    public Task<CourseEditionDto> GetCourseEdition(string courseId, string termId);

    public Task<string> GetCourseUnitTypeId(string id);

    public Task<string> GetCourseUnitTermId(string id);

    public Task<IDictionary<string, ClassTypeDto>> GetClassTypes();

    public Task<UserCoursesDto> GetUserCourses();

    public Task<CourseScheduleItemDto[]> GetCourseSchedule(string courseUnitId, int groupNumber);
}
