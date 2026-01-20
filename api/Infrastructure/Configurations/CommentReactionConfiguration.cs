using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CommentReactionConfiguration : IEntityTypeConfiguration<CommentReaction>
    {
        public void Configure(EntityTypeBuilder<CommentReaction> builder)
        {
            builder.HasKey(cr => new { cr.CommentId, cr.UserId, cr.ReactionType });

           builder.HasOne<Comment>()
                  .WithMany()
                  .HasForeignKey(cr => cr.CommentId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<AppUser>()
                   .WithMany()
                   .HasForeignKey(cr => cr.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(cr=> cr.ReactionType)
                .HasConversion<int>()
                   .IsRequired();

            builder.HasIndex(cr => cr.CommentId);
        }
    }
}