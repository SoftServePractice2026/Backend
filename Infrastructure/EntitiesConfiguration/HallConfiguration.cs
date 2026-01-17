using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.EntitiesConfiguration
{
    public class HallConfiguration : IEntityTypeConfiguration<HallEntity>
    {
        public void Configure(EntityTypeBuilder<HallEntity> builder)
        {
            builder.ToTable("hall");

            builder.HasKey(h => h.Id);
            builder.Property(h => h.Id)
                .HasColumnName("hall_id");

            builder.Property(h => h.Name)
                .HasColumnName("name")
                .IsRequired();
            builder.HasIndex(h => h.Name)
                .IsUnique();

            builder.Property(h => h.IsActive)
                .HasColumnName("is_active")
                .IsRequired();

            builder.Property(h => h.HallSize)
               .HasColumnName("hall_size")
               .HasConversion<int>()
               .IsRequired();
        }
    }
}