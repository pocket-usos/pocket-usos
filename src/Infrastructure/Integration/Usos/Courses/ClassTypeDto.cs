namespace App.Infrastructure.Integration.Usos.Courses;

public class ClassTypeDto
{
    public required string Id { get; set; }

    public required IDictionary<string, string> Name { get; set; }
}
