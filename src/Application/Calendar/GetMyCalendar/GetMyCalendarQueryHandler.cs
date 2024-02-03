using App.Application.Configuration.Queries;
using App.Domain.Calendar;

namespace App.Application.Calendar.GetMyCalendar;

public class GetMyCalendarQueryHandler(ICalendarRepository calendarRepository) : IQueryHandler<GetMyCalendarQuery, IEnumerable<CalendarItem>>
{
    public async Task<IEnumerable<CalendarItem>> Handle(GetMyCalendarQuery query, CancellationToken cancellationToken)
    {
        var calendar = await calendarRepository.GetCalendar(query.Start, query.Days);

        return calendar;
    }
}
