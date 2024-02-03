using App.Application.Contracts;
using App.Domain.Calendar;

namespace App.Application.Calendar.GetMyCalendar;

public class GetMyCalendarQuery(DateOnly? Start, int? Days) : QueryBase<IEnumerable<CalendarItem>>
{
    public DateOnly? Start { get; set; } = Start;

    public int? Days { get; set; } = Days;
}
