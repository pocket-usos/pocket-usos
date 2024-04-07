using App.Domain.BuildingBlocks;

namespace App.Domain.Notifications;

public class NotificationId(Guid value) : TypedIdValueBase(value);
