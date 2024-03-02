namespace App.Infrastructure.Integration.Usos.Courses;

public class CourseUnitDto
{
    public required string Id { get; set; }

    public required string ClasstypeId { get; set; }

    public required GroupDto[] Groups { get; set; }
}

public class CourseUnitTypeIdDto
{
    public required string ClasstypeId { get; set; }
}

public class CourseUnitTermIdDto
{
    public required string TermId { get; set; }
}

public class GroupDto
{
    public required int CourseUnitId { get; set; }

    public required int GroupNumber { get; set; }

    public required IDictionary<string, string> ClassType { get; set; }

    public required string ClassTypeId { get; set; }

    public required string CourseId { get; set; }

    public required IDictionary<string, string> CourseName { get; set; }

    public required string TermId { get; set; }

    public required string CourseLangId { get; set; }

    public required LecturerDto[] Lecturers { get; set; }

    public required ParticipantDto[] Participants { get; set; }
}
