using App.Application.Contracts;

namespace App.Application.Grades.GetGradesDistribution;

public class GetGradesDistributionQuery(string examId) : QueryBase<GradesDistributionItem[]>
{
    public string ExamId { get; set; } = examId;
}
