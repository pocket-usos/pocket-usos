using App.Application.Configuration.Queries;

namespace App.Application.Schedule.GetMySchedule;

public class GetMyScheduleQueryHandler(IScheduleRepository scheduleRepository) : IQueryHandler<GetMyScheduleQuery, IEnumerable<ScheduleItem>>
{
    public async Task<IEnumerable<ScheduleItem>> Handle(GetMyScheduleQuery query, CancellationToken cancellationToken)
    {
        var schedule = await scheduleRepository.GetSchedule(query.Start, query.Days);

        return schedule;
    }
}
