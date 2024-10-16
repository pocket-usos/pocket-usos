using App.Domain.Institutions;
using App.Domain.Notifications;
using App.Domain.UserAccess.Authentication;
using App.Infrastructure.Domain.Institutions;
using App.Infrastructure.Domain.Notifications;
using App.Infrastructure.Domain.UserAccess;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure;

public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public required DbSet<AuthenticationSession> AuthenticationSessions { get; set; }

    public required DbSet<Institution> Institutions { get; set; }

    public required DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuthenticationSessionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InstitutionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationEntityTypeConfiguration());
    }
}
