using App.Application.Configuration.Queries;
using App.Domain.Calendar;

namespace App.Application.Calendar.GetMyTerms;

public class GetMyTermsQueryHandler(ICalendarRepository calendarRepository) : IQueryHandler<GetMyTermsQuery, IEnumerable<Term>>
{
    public async Task<IEnumerable<Term>> Handle(GetMyTermsQuery query, CancellationToken cancellationToken)
    {
        return await calendarRepository.GetTerms();
    }
}
