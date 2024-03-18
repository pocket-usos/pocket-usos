using App.Application.Configuration.Queries;

namespace App.Application.Grades.GetGradesDistribution;

public class GetGradesDistributionQueryHandler(IGradesRepository gradesRepository) : IQueryHandler<GetGradesDistributionQuery, GradesDistributionItem[]>
{
    public async Task<GradesDistributionItem[]> Handle(GetGradesDistributionQuery query, CancellationToken cancellationToken)
    {
        return await gradesRepository.GetGradesDistribution(query.ExamId);
    }
}
