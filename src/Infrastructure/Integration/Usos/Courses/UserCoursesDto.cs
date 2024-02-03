namespace App.Infrastructure.Integration.Usos.Courses;

public class UserCoursesDto
{
    public required IDictionary<string, CourseEditionDto[]> CourseEditions { get; set; }
}

public class CourseEditionDto
{
    public required string CourseId { get; set; }

    public required IDictionary<string, string> CourseName { get; set; }

    public required string TermId { get; set; }

    public required UserGroupDto[] UserGroups { get; set; }
}

public class UserGroupDto
{
    public required string CourseUnitId { get; set; }

    public required int GroupNumber { get; set; }

    public required IDictionary<string, string> ClassType { get; set; }

    public required string ClassTypeId { get; set; }

    public required string CourseLangId { get; set; }

    public required LecturerDto[] Lecturers { get; set; }

    public required ParticipantDto[] Participants { get; set; }
}

public class LecturerDto
{
    public required string Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }
}

public class ParticipantDto
{
    public required string Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }
}
