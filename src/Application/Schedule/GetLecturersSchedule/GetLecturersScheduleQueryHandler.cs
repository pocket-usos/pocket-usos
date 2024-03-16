using App.Application.Configuration.Queries;

namespace App.Application.Schedule.GetLecturersSchedule;

public class GetLecturersScheduleQueryHandler(IScheduleRepository scheduleRepository) : IQueryHandler<GetLecturersScheduleQuery, IEnumerable<ScheduleItem>>
{
    public async Task<IEnumerable<ScheduleItem>> Handle(GetLecturersScheduleQuery query, CancellationToken cancellationToken)
    {
        var schedule = await scheduleRepository.GetLecturerSchedule(query.UserId, query.Start, query.Days);

        return schedule;
    }
}
