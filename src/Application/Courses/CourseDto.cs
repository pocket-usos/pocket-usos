using App.Domain.Courses;

namespace App.Application.Courses;

public class CourseDto
{
    public required string Id { get; set; }

    public required string UnitId { get; set; }

    public required string Name { get; set; }

    public required string Term { get; set; }

    public required int GroupNumber { get; set; }

    public required ClassType ClassType { get; set; }

    public ScheduleDto? Schedule { get; set; }

    public required List<LecturerDto> Lecturers { get; set; }

    public List<ParticipantDto>? Participants { get; set; } = null;
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

public class ScheduleDto
{
    public required List<ScheduleItemDto> Items { get; set; }

    public required int ClassesCount { get; set; }

    public required int ClassesCompleted { get; set; }
}

public class ScheduleItemDto
{
    public required DateTime Start { get; set; }

    public required DateTime End { get; set; }
}
