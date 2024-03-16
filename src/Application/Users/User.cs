namespace App.Application.Users;

public class User
{
    public string Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string? Email { get; set; }

    public string Status { get; set; }

    public string Sex { get; set; }

    public string PhotoUrl { get; set; }

    public string? Title { get; set; }

    public string? OfficeHoursInformation { get; set; }

    public IEnumerable<ConductedCourse>? Courses { get; set; }

    public class ConductedCourse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public CourseTerm Term { get; set; }
    }

    public class CourseTerm
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
