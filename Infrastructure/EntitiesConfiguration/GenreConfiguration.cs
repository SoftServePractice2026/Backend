using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntitiesConfiguration
{
    public class GenreConfiguration : IEntityTypeConfiguration<GenreEntity>
    {
        public void Configure(EntityTypeBuilder<GenreEntity> builder)
        {
            builder.ToTable("genre");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("genre_id");

            builder.Property(a => a.Name)
               .HasMaxLength(50)
               .IsRequired();
        }
    }
}
