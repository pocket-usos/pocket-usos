namespace App.Infrastructure.Integration.Usos.TimeTable;

public interface ITimeTableProvider
{
    Task<IEnumerable<TimeTableItemDto>> GetUserTimeTable(DateOnly? start, int? days);
}
