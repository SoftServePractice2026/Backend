using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;


namespace Infrastructure.EntitiesConfiguration
{

    public class ViewHistoryConfiguration : IEntityTypeConfiguration<ViewHistoryEntity>
    {
        public void Configure(EntityTypeBuilder<ViewHistoryEntity> builder)
        {
            builder.ToTable("history");
            
            builder.HasKey(h => h.Id);
            builder.Property(h => h.Id)
                .HasColumnName("history_id");
            
            
            // builder.Property(h => h.UserId)
            //     .HasColumnName("user_id")
            //     .IsRequired();
            
            
            builder.Property(h => h.MovieId)
                .HasColumnName("movie_id")
                .IsRequired();
            
            
            builder.Property(h => h.ViewedAt)
                .HasColumnName("viewed_at")
                .HasColumnType("timestamp")
                .IsRequired();
            
           
            // builder.HasOne(h => h.User)
            //     .WithMany(u => u.ViewHistory)
            //     .HasForeignKey(h => h.UserId)
            //     .OnDelete(DeleteBehavior.Cascade);
            
            
            builder.HasOne(h => h.Movie)
                .WithMany(m => m.MoviesViewed)
                .HasForeignKey(h => h.MovieId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}