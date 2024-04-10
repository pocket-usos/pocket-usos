namespace App.API.Grades.Requests;

public class NewGradeEvent
{
    public string Event_Type { get; set; }

    public NewGradeEventEntry[] Entry { get; set; }
}

public class NewGradeEventEntry
{
    public int Time { get; set; }

    public string[] Related_User_Ids { get; set; }

    public string Operation { get; set; }

    public string Exam_Id { get; set; }

    public int Exam_Session_Number { get; set; }
}
