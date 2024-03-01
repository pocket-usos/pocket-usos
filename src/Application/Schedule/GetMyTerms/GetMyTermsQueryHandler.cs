using App.Application.Configuration.Queries;

namespace App.Application.Schedule.GetMyTerms;

public class GetMyTermsQueryHandler(IScheduleRepository scheduleRepository) : IQueryHandler<GetMyTermsQuery, IEnumerable<Term>>
{
    public async Task<IEnumerable<Term>> Handle(GetMyTermsQuery query, CancellationToken cancellationToken)
    {
        return await scheduleRepository.GetTerms();
    }
}
