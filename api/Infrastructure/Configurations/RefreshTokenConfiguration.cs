using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .UseIdentityColumn();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.JwtTokenId).IsRequired();
            builder.Property(x => x.ExpiresAt).HasColumnType("datetime");
            builder.Property(x => x.Refresh_Token).IsRequired();

        }
    }
}
