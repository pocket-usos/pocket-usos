namespace App.Infrastructure.Integration.Usos;

internal record Scope(string Value)
{
    public static Scope CourseTests => new("crstests");
    
    public static Scope Email => new("email");
    
    public static Scope Events => new("events");
    
    public static Scope Grades => new("grades");
    
    public static Scope MobileNumbers => new("mobile_numbers");
    
    public static Scope OfflineAccess => new("offline_access");
    
    public static Scope OtherEmails => new("other_emails");
    
    public static Scope Personal => new("personal");
    
    public static Scope Photo => new("photo");
    
    public static Scope PlacementTests => new("placement_tests");
    
    public static Scope StudentExams => new("student_exams");
    
    public static Scope Studies => new("studies");
    
    public static Scope[] All => [CourseTests, Email, Events, Grades, MobileNumbers, OfflineAccess, OtherEmails, Personal, Photo, PlacementTests, StudentExams, Studies];
    
    public static string[] AllValues => All.Select(scope => scope.Value).ToArray(); 
}
