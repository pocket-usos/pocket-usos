namespace App.Infrastructure.Integration.Usos.TimeTable;

public class TimeTableItemDto
{
    public required string StartTime { get; set; }

    public required string EndTime { get; set; }

    public required IDictionary<string, string> Name { get; set; }

    public required string CourseId { get; set; }

    public required string ClasstypeId { get; set; }

    public required IDictionary<string, string> ClasstypeName { get; set; }

    public required int[] LecturerIds { get; set; }

    public required int GroupNumber { get; set; }

    public int? RoomId { get; set; }

    public required string RoomNumber { get; set; }

    public required int UnitId { get; set; }
}
