using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .UseIdentityColumn();

            builder.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()")
                .IsRequired();

            builder.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(c => c.AuthorUserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

           builder.HasOne<WorkItem>()
                .WithMany()
                .HasForeignKey(c => c.WorkItemId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Comment>()
                .WithMany()
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(c => c.WorkItemId);
        }
    }
}