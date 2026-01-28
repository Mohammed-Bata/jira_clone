using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
    {
        public void Configure(EntityTypeBuilder<WorkItem> builder)
        {
            
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Id)
                .UseIdentityColumn();

            builder.Property(w => w.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(w => w.Description)
                .HasMaxLength(1000);

            builder.Property(w => w.Order)
                .HasColumnType("float")
                .IsRequired();

            builder.Property(w => w.Priority)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(w => w.Type)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(w => w.DueDate)
                .HasColumnType("date")
               .IsRequired(false);

            builder.HasOne<ProjectColumn>()        
           .WithMany()                      
           .HasForeignKey(w => w.ProjectColumnId)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(w => w.AuthorUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(w => w.AssignedToUserId)
                .OnDelete(DeleteBehavior.SetNull);


            builder.HasIndex(w => w.ProjectColumnId);
        }
    }
}