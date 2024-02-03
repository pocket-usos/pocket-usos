namespace App.Infrastructure.Integration.Usos.Grades;

public class GradesDistributionDto
{
    public required ExamGradesDistributionItemDto[] GradesDistribution { get; set; }
}

public class ExamGradesDistributionItemDto
{
    public required decimal Percentage { get; set; }

    public required string GradeSymbol { get; set; }
}
