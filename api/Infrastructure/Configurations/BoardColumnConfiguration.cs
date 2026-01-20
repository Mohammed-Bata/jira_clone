using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class BoardColumnConfiguration : IEntityTypeConfiguration<BoardColumn>
    {
        public void Configure(EntityTypeBuilder<BoardColumn> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .UseIdentityColumn();

            builder.Property(b => b.Order)
                .IsRequired();

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}