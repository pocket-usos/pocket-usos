namespace App.Infrastructure.Integration.Usos.Courses;

public class CourseDto
{
    public required string Id { get; set; }

    public required IDictionary<string, string> Name { get; set; }
}
