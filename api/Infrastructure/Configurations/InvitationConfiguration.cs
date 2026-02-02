using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class InvitationConfiguration:IEntityTypeConfiguration<Invitation>
    {
        public void Configure(EntityTypeBuilder<Invitation> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .UseIdentityColumn();

            builder.HasOne<Project>()
                .WithMany()
                .HasForeignKey(i => i.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(i => i.Email)
                .IsRequired()
                .HasMaxLength(256);


            builder.Property(i => i.Role)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(i => i.Token)
                .IsRequired();

            builder.Property(i => i.IsAccepted)
                .HasDefaultValue(false);

            builder.Property(i => i.ExpiresAt)
                .IsRequired();


            builder.HasIndex(i=>i.Token)
                .IsUnique();
            
        }
    }
}
