namespace App.API.Calendar.Requests;

public class GetMyCalendarRequest
{
    public DateOnly? Start { get; set; }

    public int? Days { get; set; }
}
