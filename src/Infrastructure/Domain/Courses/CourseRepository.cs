using App.Domain.Courses;
using App.Infrastructure.Integration.Usos.Courses;

namespace App.Infrastructure.Domain.Courses;

public class CourseRepository(ICoursesProvider coursesProvider) : ICourseRepository
{
    public async Task<Course> GetCourse(string id)
    {
        var course = await coursesProvider.GetCourse(id);

        return new Course(course.Id, course.Name["pl"]);
    }

    public async Task<CourseUnit> GetCourseUnit(string id)
    {
        var courseUnit = await coursesProvider.GetCourseUnit(id);
        var classTypes = await coursesProvider.GetClassTypes();
        var classTypeDto = classTypes[courseUnit.ClasstypeId];
        var classType = new ClassType(classTypeDto.Id, classTypeDto.Name["pl"]);

        return new CourseUnit(courseUnit.Id, classType);
    }
}
