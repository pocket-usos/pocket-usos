using App.Application.Contracts;

namespace App.Application.Grades.NotifyAboutNewGrade;

public class NotifyAboutNewGradeCommand(Guid institutionId, NewGradeNotificationEntry[] notificationEntries) : CommandBase
{
    public Guid InstitutionId { get; init; } = institutionId;

    public NewGradeNotificationEntry[] NotificationEntries { get; init; } = notificationEntries;
}

public class NewGradeNotificationEntry
{
    public int TimeStamp { get; set; }

    public string[] RelatedUserIds { get; set; }

    public string ExamId { get; set; }

    public int ExamSessionNumber { get; set; }
}
