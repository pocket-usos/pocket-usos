using App.Application.Configuration;
using App.Infrastructure.Configuration.DataAccess;
using App.Infrastructure.Integration.Client;
using App.Infrastructure.Integration.Requests;

namespace App.Infrastructure.Integration.Usos.TimeTable;

internal class UsosTimeTableProvider(IUsosHttpClient client, IExecutionContextAccessor context, ICacheProvider cache) : ITimeTableProvider
{
    public async Task<IEnumerable<TimeTableItemDto>> GetStaffTimeTable(string userId, DateOnly start, int days)
    {
        var timeTable = await cache.GetAsync<IEnumerable<TimeTableItemDto>>($"usos-staff-{userId}-timetable-{start.ToString("yyyy-MM-dd")}-{days}");

        if (timeTable is not null)
        {
            return timeTable;
        }

        var request = Request.Get("services/tt/staff")
            .WithQueryParameter("user_id", userId)
            .WithQueryParameter("fields",
                "start_time|end_time|name|course_id|course_name|classtype_id|classtype_name|lecturer_ids|group_number|building_name|building_id|room_number|room_id|unit_id|classtype_id|cgwm_id|frequency")
            .WithQueryParameter("start", start.ToString("yyyy-MM-dd"))
            .WithQueryParameter("days", days.ToString());

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        timeTable = response.Content!.As<IEnumerable<TimeTableItemDto>>().ToList();

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

        var request = Request.Get("services/tt/user")
            .WithQueryParameter("fields",
                "start_time|end_time|name|course_id|course_name|classtype_id|classtype_name|lecturer_ids|group_number|building_name|building_id|room_number|room_id|unit_id|classtype_id|cgwm_id|frequency")
            .WithQueryParameter("start", start.ToString("yyyy-MM-dd"))
            .WithQueryParameter("days", days.ToString());

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        timeTable = response.Content!.As<IEnumerable<TimeTableItemDto>>().ToList();

        await cache.SetAsync($"usos-user-{context.SessionId}-timetable-{start.ToString("yyyy-MM-dd")}-{days}", timeTable, options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
        });

        return timeTable;
    }
}
