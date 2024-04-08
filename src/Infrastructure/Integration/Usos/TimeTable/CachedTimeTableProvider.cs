using App.Application.Configuration;
using App.Infrastructure.Configuration.DataAccess;

namespace App.Infrastructure.Integration.Usos.TimeTable;

internal class CachedTimeTableProvider(ITimeTableProvider timeTableProvider, ICacheProvider cache, IExecutionContextAccessor context) : ITimeTableProvider
{
    public async Task<IEnumerable<TimeTableItemDto>> GetStaffTimeTable(string userId, DateOnly start, int days)
    {
        var timeTable = await cache.GetAsync<IEnumerable<TimeTableItemDto>>($"usos-staff-{userId}-timetable-{start.ToString("yyyy-MM-dd")}-{days}");

        if (timeTable is not null)
        {
            return timeTable;
        }

        timeTable = (await timeTableProvider.GetStaffTimeTable(userId, start, days)).ToList();

        await cache.SetAsync($"usos-staff-{userId}-timetable-{start.ToString("yyyy-MM-dd")}-{days}", timeTable, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
        });

        return timeTable;
    }

    public async Task<IEnumerable<TimeTableItemDto>> GetUserTimeTable(DateOnly start, int days)
    {
        var timeTable = await cache.GetAsync<IEnumerable<TimeTableItemDto>>($"usos-user-{context.SessionId}-timetable-{start.ToString("yyyy-MM-dd")}-{days}");

        if (timeTable is not null)
        {
            return timeTable;
        }

        timeTable = (await timeTableProvider.GetUserTimeTable(start, days)).ToList();

        await cache.SetAsync($"usos-user-{context.SessionId}-timetable-{start.ToString("yyyy-MM-dd")}-{days}", timeTable, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
        });

        return timeTable;
    }
}
