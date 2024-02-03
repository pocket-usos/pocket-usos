namespace App.Domain.Courses;

public class Course(string id, string name)
{
    public string Id { get; set; } = id;

    public string Name { get; set; } = name;
}
