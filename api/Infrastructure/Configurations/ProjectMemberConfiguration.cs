using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class ProjectMemberConfiguration:IEntityTypeConfiguration<ProjectMember>
    {
        public void Configure(EntityTypeBuilder<ProjectMember> builder)
        {
            builder.HasKey(pm => new { pm.ProjectId, pm.UserId });
            builder.HasOne(pm=>pm.Project)
                .WithMany(p=>p.Members)
                .HasForeignKey(pm => pm.ProjectId)
                .IsRequired();
            builder.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(pm => pm.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
            builder.Property(pm => pm.Role)
                .HasConversion<int>()
                .IsRequired();
            
        }
    }
}
