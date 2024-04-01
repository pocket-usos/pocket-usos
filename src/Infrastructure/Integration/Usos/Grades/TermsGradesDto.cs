namespace App.Infrastructure.Integration.Usos.Grades;

public class TermCourseDto
{
    public required IDictionary<string, IDictionary<string, GradeDto?>[]> CourseUnitsGrades { get; set; }
}

public class GradeDto
{
    public required string ValueSymbol { get; set; }

    public required bool Passes { get; set; }

    public required IDictionary<string, string> ValueDescription { get; set; }

    public required int ExamId { get; set; }

    public required int ExamSessionNumber { get; set; }

    public required string CountsIntoAverage { get; set; }

    public required string Comment { get; set; }

    public required string GradeTypeId { get; set; }

    public required string DateModified { get; set; }

    public required string DateAcquisition { get; set; }

    public required ModificationAuthorDto? ModificationAuthor { get; set; }

    public UnitDto? Unit { get; set; }
}

public class ModificationAuthorDto
{
    public required string Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }
}

public class UnitDto
{
    public required IDictionary<string, string> CourseName { get; set; }

    public required string TermId { get; set; }
}
