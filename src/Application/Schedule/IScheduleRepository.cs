namespace App.Application.Schedule;

public interface IScheduleRepository
{
    Task<IEnumerable<ScheduleItem>> GetSchedule(DateOnly? start, int? days);

    Task<IEnumerable<Term>> GetTerms();
}
