using App.Application.Courses;
using App.Domain.Courses;
using App.Infrastructure.Integration.Usos.Courses;
using CourseDto = App.Application.Courses.CourseDto;
using LecturerDto = App.Application.Courses.LecturerDto;
using ParticipantDto = App.Application.Courses.ParticipantDto;

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

    public async Task<List<CourseDto>> GetMyCoursesForTerm(string termId, bool withSchedule = false)
    {
        var userCourses = await coursesProvider.GetUserCourses();
        var termCourses = userCourses.CourseEditions[termId];

        var courses = new List<CourseDto>();
        foreach (var courseEdition in termCourses)
        {
            foreach (var userGroup in courseEdition.UserGroups)
            {
                var lecturers = userGroup.Lecturers.Select(lecturer => new LecturerDto
                {
                    Id = lecturer.Id,
                    FirstName = lecturer.FirstName,
                    LastName = lecturer.LastName,
                }).ToList();

                ScheduleDto? schedule = null;
                if (withSchedule)
                {
                    var courseSchedule = await coursesProvider.GetCourseSchedule(userGroup.CourseUnitId, userGroup.GroupNumber);
                    schedule = new ScheduleDto
                    {
                        Items = courseSchedule.Select(s => new ScheduleItemDto
                        {
                            Start = DateTime.Parse(s.StartTime),
                            End = DateTime.Parse(s.EndTime),
                        }).ToList(),
                        ClassesCount = courseSchedule.Length,
                        ClassesCompleted = courseSchedule.Count(s => DateTime.Parse(s.EndTime) < DateTime.Now),
                    };
                }

                courses.Add(new CourseDto
                {
                    Id = userGroup.CourseId,
                    UnitId = userGroup.CourseUnitId,
                    Name = userGroup.CourseName["pl"],
                    Term = userGroup.TermId,
                    GroupNumber = userGroup.GroupNumber,
                    ClassType = new ClassType(userGroup.ClassTypeId,
                        userGroup.ClassType["pl"]),
                    Lecturers = lecturers,
                    Schedule = schedule,
                    Participants = userGroup.Participants.Select(p => new ParticipantDto
                    {
                        Id = p.Id,
                        FirstName = p.FirstName,
                        LastName = p.LastName
                    }).ToList(),
                });
            }
        }

        return courses;
    }
}
