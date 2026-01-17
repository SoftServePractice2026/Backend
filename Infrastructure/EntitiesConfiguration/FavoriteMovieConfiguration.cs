using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntitiesConfiguration
{
    public class FavoriteMovieConfiguration : IEntityTypeConfiguration<FavoriteMovieEntity>
    {
        public void Configure(EntityTypeBuilder<FavoriteMovieEntity> builder)
        {
            builder.ToTable("favorite_movies");

            builder.HasKey(f => f.Id);
            builder.Property(f => f.Id)
                .HasColumnName("favorite_movies_id");

            builder.Property(f => f.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(f => f.MovieId)
                .HasColumnName("movie_id")
                .IsRequired();

            builder.Property(f => f.AddedAt)
                .HasColumnName("added_at")
                .HasColumnType("timestamp")
                .IsRequired();

            builder.HasOne(f => f.Movie)
                .WithMany(m => m.FavoriteMovies)
                .HasForeignKey(f => f.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.ApplicationUser)
                .WithMany(u => u.FavoriteMovies)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
