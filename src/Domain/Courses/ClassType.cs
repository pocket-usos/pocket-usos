namespace App.Domain.Courses;

public class ClassType(string id, string name)
{
    public string Id { get; set; } = id;

    public string Name { get; set; } = name;
}
