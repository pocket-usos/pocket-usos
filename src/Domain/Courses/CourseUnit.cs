namespace App.Domain.Courses;

public class CourseUnit(string id, ClassType type)
{
    public string Id { get; set; } = id;

    public ClassType Type { get; set; } = type;
}
