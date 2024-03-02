namespace App.API.Courses.Requests;

public class GetCoursesForTermRequest
{
    public string Term { get; set; }

    public bool WithSchedule { get; set; } = false;
}
