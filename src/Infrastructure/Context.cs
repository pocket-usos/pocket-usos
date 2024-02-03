using App.Domain.UserAccess.Authentication;
using App.Infrastructure.Domain.UserAccess;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure;

public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public required DbSet<AuthenticationSession> AuthenticationSessions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuthenticationSessionEntityTypeConfiguration());
    }
}