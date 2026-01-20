using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class ProjectConfiguration: IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .UseIdentityColumn();
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(p => p.Description)
                .HasMaxLength(1000);
            builder.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(p => p.OwnerId)
                .IsRequired();
            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("sysdatetimeoffset()")
                .IsRequired();
        }
    }
}
