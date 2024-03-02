namespace App.Application.Schedule;

public class Term
{
    public string Id { get; set; }

    public string Name { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }
}
