using App.Domain.Courses;
using App.Domain.Users;
using App.Infrastructure.Integration.Usos.Courses;

namespace App.Infrastructure.Domain.Courses;

public class CourseRepository(ICoursesProvider coursesProvider, IUserRepository userRepository) : ICourseRepository
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

    public async Task<List<Course>> GetMyCoursesForTerm(string termId)
    {
        var userCourses = await coursesProvider.GetUserCourses();
        var termCourses = userCourses.CourseEditions[termId];

        var courses = new List<Course>();
        foreach (var courseEdition in termCourses)
        {
            var course = new Course(courseEdition.CourseId, courseEdition.CourseName["pl"]);
            course.Language = courseEdition.UserGroups.FirstOrDefault()?.CourseLangId;
            course.Term = termId;

            foreach (var userGroup in courseEdition.UserGroups)
            {
                var lecturers = await GetLecturers(userGroup);
                var participants = await GetParticipants(userGroup);

                course.Groups.Add(new CourseGroup
                {
                    Number = userGroup.GroupNumber,
                    ClassType = new ClassType(userGroup.ClassTypeId, userGroup.ClassType["pl"]),
                    Lecturers = lecturers,
                    Participants = participants
                });
            }

            courses.Add(course);
        }

        return courses;
    }

    private async Task<List<Lecturer>> GetLecturers(UserGroupDto userGroup)
    {
        var userIds = userGroup.Lecturers.Select(l => l.Id).ToArray();
        var users = await userRepository.GetMultipleAsync(userIds);

        return users.Select(u => new Lecturer
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            PhotoUrl = u.PhotoUrl
        }).ToList();
    }

    private async Task<List<Participant>> GetParticipants(UserGroupDto userGroup)
    {
        var userIds = userGroup.Participants.Select(l => l.Id).ToArray();
        var userIdsChunks = userIds.Select((id, index) => new { Value = id, Index = index })
            .GroupBy(x => x.Index / 50)
            .Select(x => x.Select(y => y.Value).ToArray())
            .ToArray();

        var participants = new List<Participant>();
        foreach (var userIdsChunk in userIdsChunks)
        {
            var users = await userRepository.GetMultipleAsync(userIdsChunk);
            participants.AddRange(users.Select(u => new Participant
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhotoUrl = u.PhotoUrl
            }));
        }

        return participants;
    }
}
