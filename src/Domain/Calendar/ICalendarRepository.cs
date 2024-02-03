namespace App.Domain.Calendar;

public interface ICalendarRepository
{
    Task<IEnumerable<CalendarItem>> GetCalendar(DateOnly? start, int? days);
}
