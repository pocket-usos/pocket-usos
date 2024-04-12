using System.Text.RegularExpressions;
using App.Application.Configuration.Commands;
using App.Application.Notifications;
using App.Domain.Institutions;
using App.Domain.Notifications;
using App.Domain.UserAccess.Authentication;
using ILogger = Serilog.ILogger;

namespace App.Application.Grades.NotifyAboutNewGrade;

public class NotifyAboutNewGradeCommandHandler(
    IAuthenticationSessionRepository authenticationSessionRepository,
    IGradesRepository gradesRepository,
    INotificationRepository notificationRepository,
    IPushNotificationSender pushNotificationSender,
    ILogger logger) : ICommandHandler<NotifyAboutNewGradeCommand>
{
    public async Task Handle(NotifyAboutNewGradeCommand command, CancellationToken cancellationToken)
    {
        foreach (var entry in command.NotificationEntries)
        {
            foreach (var userId in entry.RelatedUserIds)
            {
                var session = await authenticationSessionRepository.GetByUserIdOrDefaultAsync(userId);

                if (session is null) continue;

                try
                {
                    var grade = await gradesRepository.GetGrade(session.Id, entry.ExamId, entry.ExamSessionNumber);

                    var notification = new Notification(
                        userId,
                        new InstitutionId(command.InstitutionId),
                        NotificationType.Grades,
                        GenerateNotificationContent(grade));

                    await notificationRepository.AddAsync(notification);

                    await pushNotificationSender.SendAsync(new OneSignalPushNotification
                    {
                        Id = notification.Id.Value,
                        SessionId = session.Id.Value,
                        HeadingInEnglish = "New grade",
                        HeadingInPolish = "Nowa ocena",
                        ContentInEnglish = Regex.Replace(notification.Content.En, @"<[^>]*>", string.Empty),
                        ContentInPolish = Regex.Replace(notification.Content.Pl, @"<[^>]*>", string.Empty),
                    });
                }
                catch (Exception exception)
                {
                    logger.Warning(
                        "Notification about new grade wasn't sent to user with ID {UserId} due to error: {Message}",
                        userId,
                        exception.Message);
                }
            }
        }

        await notificationRepository.SaveAsync();
    }

    private NotificationContent GenerateNotificationContent(SessionGrade grade)
    {
        var englishValue = $"You've received <b>\"{grade.Grade}\"</b> grade from course <b>\"{grade.Unit?.CourseName["en"]}\"</b>.";
        var polishValue = $"Otrzymałeś ocenę <b>\"{grade.Grade}\"</b> z przedmiotu <b>\"{grade.Unit?.CourseName["pl"]}\"</b>.";

        return new NotificationContent(En: englishValue, Pl: polishValue);
    }
}
