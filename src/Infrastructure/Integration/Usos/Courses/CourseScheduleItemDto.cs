namespace App.Infrastructure.Integration.Usos.Courses;

public class CourseScheduleItemDto
{
    public required string StartTime { get; set; }

    public required string EndTime { get; set; }

    public required IDictionary<string, string> Name { get; set; }
}
