using App.Application.Courses;
using App.Application.Shared;
using App.Infrastructure.Integration.Usos.Courses;

namespace App.Infrastructure.Application.Courses;

public class CourseRepository(ICoursesProvider coursesProvider) : ICourseRepository
{
    public async Task<Course> GetCourse(string courseId, string courseUnitId)
    {
        var termId = await coursesProvider.GetCourseUnitTermId(courseUnitId);
        var courseEdition = await coursesProvider.GetCourseEdition(courseId, termId);

        var group = courseEdition.UserGroups[0];

        var lecturers = group.Lecturers.Select(l => new Lecturer
        {
            Id = l.Id, FirstName = l.FirstName, LastName = l.LastName
        }).ToList();

        var participants = group.Participants.Select(p => new Participant
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName
        }).ToList();

        var courseSchedule = await coursesProvider.GetCourseSchedule(group.CourseUnitId, group.GroupNumber);
        var schedule = new ScheduleDto
        {
            Items = courseSchedule.Select(s => new ScheduleItemDto
            {
                Start = DateTime.Parse(s.StartTime),
                End = DateTime.Parse(s.EndTime),
            }).ToList(),
            ClassesCount = courseSchedule.Length,
            ClassesCompleted = courseSchedule.Count(s => DateTime.Parse(s.EndTime) < DateTime.Now),
        };

        return new Course
        {
            Id = group.CourseId,
            UnitId = group.CourseUnitId,
            Name = group.CourseName["pl"],
            Term = group.TermId,
            GroupNumber = group.GroupNumber,
            ClassType = new ClassType(group.ClassTypeId, group.ClassType["pl"]),
            Schedule = schedule,
            Lecturers = lecturers,
            Participants = participants
        };
    }

    public async Task<List<Course>> GetMyCoursesForTerm(string termId, bool withSchedule = false)
    {
        var userCourses = await coursesProvider.GetUserCourses();
        var termCourses = userCourses.CourseEditions[termId];

        var courses = new List<Course>();
        foreach (var courseEdition in termCourses)
        {
            foreach (var userGroup in courseEdition.UserGroups)
            {
                var lecturers = userGroup.Lecturers.Select(lecturer => new Lecturer
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

                courses.Add(new Course
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
                    Participants = userGroup.Participants.Select(p => new Participant
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
