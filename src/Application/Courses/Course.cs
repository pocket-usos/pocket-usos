using App.Application.Shared;

namespace App.Application.Courses;

public class Course
{
    public required string Id { get; set; }

    public required string UnitId { get; set; }

    public required string Name { get; set; }

    public required string Term { get; set; }

    public required int GroupNumber { get; set; }

    public required ClassType ClassType { get; set; }

    public Schedule? Schedule { get; set; }

    public required List<Lecturer> Lecturers { get; set; }

    public List<Participant>? Participants { get; set; } = null;
}

public class Lecturer
{
    public required string Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }
}

public class Participant
{
    public required string Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }
}

public class Schedule
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
