namespace App.API.Schedule.Requests;

public class GetMyScheduleRequest
{
    public DateOnly? Start { get; set; }

    public int? Days { get; set; }
}
