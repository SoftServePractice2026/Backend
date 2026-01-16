using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.EntitiesConfiguration
{
    public class MovieConfiguration : IEntityTypeConfiguration<MovieEntity>
    {
        public void Configure(EntityTypeBuilder<MovieEntity> builder)
        {
            builder.ToTable("movie");
            
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id)
                .HasColumnName("movie_id");
            
            
            builder.Property(m => m.Poster)
                .HasColumnName("poster");
            
            
            builder.Property(m => m.Title)
                .HasColumnName("title")
                .IsRequired();

            
            builder.Property(m => m.Description)
                .HasColumnName("description")
                .IsRequired();

            
            builder.Property(m => m.Category)
                .HasColumnName("category_id")
                .HasConversion<int>()
                .IsRequired();

            
            builder.Property(m => m.Genre)
                .HasColumnName("genre_id")
                .HasConversion<int>()
                .IsRequired();

            
            builder.Property(m => m.Duration)
                .HasColumnName("duration_min")
                .IsRequired();

            
            builder.Property(m => m.Rating)
                .HasColumnName("rating")
                .IsRequired(false);

            
            builder.Property(m => m.RentalStartDate)
                .HasColumnName("start_of_rental")
                .HasColumnType("date")
                .IsRequired();

            
            builder.Property(m => m.RentalEndDate)
                .HasColumnName("end_of_rental")
                .HasColumnType("date")
                .IsRequired();

            
            builder.Property(m => m.IsActive)
                .HasColumnName("is_active")
                .IsRequired();

            
            builder.Property(m => m.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("timezone('utc', now())")
                .IsRequired();
        }
    }
}