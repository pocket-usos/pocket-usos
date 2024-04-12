using App.Domain.Institutions;
using App.Domain.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Domain.Notifications;

public class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");
        builder.HasKey(n => n.Id);
        builder.Property<NotificationId>(n => n.Id).HasColumnName("id");

        builder.Property<InstitutionId>(n => n.InstitutionId).HasColumnName("institution_id");
        builder.Property<string>(n => n.UserId).HasColumnName("user_id");
        builder.HasIndex("UserId", "InstitutionId");

        builder.Property(n => n.WasRead).HasColumnName("was_read");
        builder.Property(n => n.CreatedAt).HasColumnName("created_at");

        builder.ComplexProperty<NotificationType>(n => n.Type, b =>
        {
            b.Property<string>(c => c.Value).HasColumnName("type");
        });

        builder.ComplexProperty<NotificationContent>(n => n.Content, b =>
        {
            b.Property<string>(c => c.Pl).HasColumnName("content_pl");
            b.Property<string>(c => c.En).HasColumnName("content_en");
        });
    }
}
