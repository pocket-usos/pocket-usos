namespace App.API.Courses.Requests;

public class GetTermCoursesRequest
{
    public string Term { get; set; }

    public bool WithSchedule { get; set; } = false;
}
