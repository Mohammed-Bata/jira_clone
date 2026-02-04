using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Id)
                .UseIdentityColumn();

            builder.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(n => n.ActorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(n => n.Message)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(n => n.CreatedAt)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()")
                .IsRequired();

            builder.Property(n => n.IsRead)
                .HasDefaultValue(false);

            builder.HasIndex(n => n.UserId);
            builder.HasIndex(n => n.IsRead);

        }
    }
}