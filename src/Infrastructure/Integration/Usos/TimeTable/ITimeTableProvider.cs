namespace App.Infrastructure.Integration.Usos.TimeTable;

public interface ITimeTableProvider
{
    Task<IEnumerable<TimeTableItemDto>> GetUserTimeTable(DateOnly? start, int? days);

    Task<IEnumerable<TimeTableItemDto>> GetStaffTimeTable(string userId, DateOnly? start, int? days);
}
