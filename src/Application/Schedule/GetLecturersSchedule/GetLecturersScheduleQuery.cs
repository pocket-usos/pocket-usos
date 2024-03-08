using App.Application.Contracts;

namespace App.Application.Schedule.GetLecturersSchedule;

public class GetLecturersScheduleQuery(string UserId, DateOnly? Start, int? Days) : QueryBase<IEnumerable<ScheduleItem>>
{
    public string UserId { get; set; } = UserId;

    public DateOnly? Start { get; set; } = Start;

    public int? Days { get; set; } = Days;
}
