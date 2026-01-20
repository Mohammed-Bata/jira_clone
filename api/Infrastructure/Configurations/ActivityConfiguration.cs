using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .UseIdentityColumn();

            builder.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .IsRequired();

            builder.Property(a => a.CreatedAt)
                .HasDefaultValueSql("sysdatetimeoffset()")
                .IsRequired();

            builder.Property(a => a.Action)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.ItemId)
                .IsRequired();

            builder.Property(a => a.ItemType)
                .HasConversion<int>()
                .IsRequired();
        }
    }
}