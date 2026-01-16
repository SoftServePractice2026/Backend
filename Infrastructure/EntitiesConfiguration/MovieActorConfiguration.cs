using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;


namespace Infrastructure.EntitiesConfiguration
{
    public class MovieActorConfiguration : IEntityTypeConfiguration<MovieActorEntity>
    {
        public void Configure(EntityTypeBuilder<MovieActorEntity> builder)
        {
            builder.ToTable("movie_actors");
            
            builder.HasKey(h => h.Id);
            builder.Property(h => h.Id)
                .HasColumnName("movie_actors_id");
            
            
            builder.Property(ma => ma.MovieId)
                .HasColumnName("movie_id")
                .IsRequired();
            
            
            builder.Property(ma => ma.ActorId)
                .HasColumnName("actor_id")
                .IsRequired();

            
            builder.HasIndex(ma => new { ma.MovieId, ma.ActorId })
                .IsUnique();


            builder.HasOne(ma => ma.Movie)
                .WithMany(m => m.ActorsInMovies)
                .HasForeignKey(ma => ma.MovieId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(ma => ma.Actor)
                .WithMany(a => a.ActorsInMovies)
                .HasForeignKey(ma => ma.ActorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}