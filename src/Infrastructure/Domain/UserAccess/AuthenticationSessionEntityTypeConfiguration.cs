using App.Domain.Institutions;
using App.Domain.UserAccess.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Domain.UserAccess;

public class AuthenticationSessionEntityTypeConfiguration : IEntityTypeConfiguration<AuthenticationSession>
{
    public void Configure(EntityTypeBuilder<AuthenticationSession> builder)
    {
        builder.ToTable("authentication_sessions");
        builder.HasKey(s => s.Id);
        builder.Property<AuthenticationSessionId>(s => s.Id).HasColumnName("id");
        builder.Property<InstitutionId>(s => s.InstitutionId).HasColumnName("institution_id");

        builder.Property<string?>(s => s.UserId).HasColumnName("user_id");

        builder.ComplexProperty<RequestToken>(s => s.RequestToken, b =>
        {
            b.Property<string>(t => t.Value).HasColumnName("request_token");
            b.Property<string>(t => t.Secret).HasColumnName("request_token_secret");
        });

        builder.OwnsOne<AccessToken>(s => s.AccessToken, b =>
        {
            b.Property<string>(t => t.Value).HasColumnName("access_token");
            b.Property<string>(t => t.Secret).HasColumnName("access_token_secret");
        });
    }
}
