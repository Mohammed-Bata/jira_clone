using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CommentMentionConfiguration : IEntityTypeConfiguration<CommentMention>
    {
        public void Configure(EntityTypeBuilder<CommentMention> builder)
        {

            builder.HasKey(cm => new { cm.CommentId, cm.MentionedUserId });

            builder.HasOne<Comment>()
                   .WithMany()
                   .HasForeignKey(cm => cm.CommentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<AppUser>()
                   .WithMany()
                   .HasForeignKey(cm => cm.MentionedUserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}