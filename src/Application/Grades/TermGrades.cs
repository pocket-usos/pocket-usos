using App.Application.Shared;

namespace App.Application.Grades;

public class TermGrades(string term)
{
    public string Term { get; set; } = term;

    public decimal AverageGrade { get; set; }

    public List<TermCourse> Courses { get; set; } = new();
}

public class TermCourse(string id, string name)
{
    public string Id { get; set; } = id;

    public string Name { get; set; } = name;

    public List<TermCourseUnit> Units { get; set; } = new();
}

public class TermCourseUnit(string id, ClassType type)
{
    public string Id { get; set; } = id;

    public ClassType Type { get; set; } = type;

    public List<SessionGrade> Grades { get; set; } = new();
}

public class SessionGrade
{
    public string SessionNumber { get; set; }

    public string ExamId { get; set; }

    public string Grade { get; set; }

    public string GradeDescription { get; set; }

    public bool Passes { get; set; }

    public bool CountsIntoAverage { get; set; }

    public string Comment { get; set; }

    public string GradeTypeId { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public GradeAuthor? ModifiedBy { get; set; }
}

public class GradeAuthor
{
    public string Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}

public class GradesDistributionItem
{
    public decimal Percentage { get; set; }

    public string Grade { get; set; }
}
