namespace App.Domain.Courses;

public class Course(string id, string name)
{
    public string Id { get; set; } = id;

    public string Name { get; set; } = name;

    public string? Language { get; set; }

    public string? Term { get; set; }

    public List<CourseGroup> Groups { get; set; } = new();
}

public class CourseGroup
{
    public int Number { get; set; }

    public ClassType ClassType { get; set; }

    public List<Lecturer> Lecturers { get; set; }

    public List<Participant> Participants { get; set; }
}

public class Lecturer
{
    public string Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string PhotoUrl { get; set; }
}

public class Participant
{
    public string Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string PhotoUrl { get; set; }
}
