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

            builder.Property(m => m.AgeRating)
                .HasColumnName("age_rating");

            builder.Property(m => m.Language)
                .HasColumnName("language");

            builder.Property(m => m.Duration)
                .HasColumnName("duration_min")
                .IsRequired();

            builder.Property(m => m.Rating)
                .HasColumnName("rating");

            builder.Property(m => m.TmdbId)
                .HasColumnName("tmdb_id");

            builder.Property(m => m.Year)
                .HasColumnName("year");

            builder.Property(m => m.Formats)
                .HasColumnName("formats");

            builder.Property(m => m.RentalStartDate)
                .HasColumnName("start_of_rental")
                .HasColumnType("date")
                .IsRequired();

            builder.Property(m => m.RentalEndDate)
                .HasColumnName("end_of_rental")
                .HasColumnType("date")
                .IsRequired();

            builder.Property(m => m.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("timezone('utc', now())")
                .IsRequired();

            builder.Property(m => m.LastModifiedAt)
                .HasColumnName("last_modified_at")
                .HasColumnType("timestamp");

            //Genre <-> Movie (many-to-many)
            builder
                .HasMany(m => m.Genres)
                .WithMany(g => g.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieGenre",
                    j => j
                        .HasOne<GenreEntity>()
                        .WithMany()
                        .HasForeignKey("genre_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<MovieEntity>()
                        .WithMany()
                        .HasForeignKey("movie_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("movie_id", "genre_id");
                        j.HasIndex("movie_id", "genre_id").IsUnique();
                    });
        }
    }
}