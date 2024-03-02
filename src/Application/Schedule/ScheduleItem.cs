using App.Application.Shared;

namespace App.Application.Schedule;

public class ScheduleItem
{
    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public string Name { get; set; }

    public string CourseId { get; set; }

    public int CourseUnitId { get; set; }

    public ClassType ClassType { get; set; }

    public List<Lecturer> Lecturers { get; set; }

    public int GroupNumber { get; set; }

    public Room Room { get; set; }
}

public class Room
{
    public int Id { get; set; }

    public string Name { get; set; }
}
