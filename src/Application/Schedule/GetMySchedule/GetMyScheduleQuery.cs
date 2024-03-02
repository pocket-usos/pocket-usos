using App.Application.Contracts;

namespace App.Application.Schedule.GetMySchedule;

public class GetMyScheduleQuery(DateOnly? Start, int? Days) : QueryBase<IEnumerable<ScheduleItem>>
{
    public DateOnly? Start { get; set; } = Start;

    public int? Days { get; set; } = Days;
}
