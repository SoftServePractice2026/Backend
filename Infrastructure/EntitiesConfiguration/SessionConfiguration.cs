using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;


namespace Infrastructure.EntitiesConfiguration
{
    public class SessionConfiguration : IEntityTypeConfiguration<SessionEntity>
    {
        public void Configure(EntityTypeBuilder<SessionEntity> builder)
        {
            builder.ToTable("session");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .HasColumnName("session_id");
            
            
            builder.Property(s => s.MovieId)
                .HasColumnName("movie_id")
                .IsRequired();
            
            
            builder.Property(s => s.HallId)
                .HasColumnName("hall_id")
                .IsRequired();


            builder.Property(s => s.StartTime)
                .HasColumnName("start_time")
                .HasColumnType("timestamp");


            builder.Property(s => s.EndTime)
                .HasColumnName("end_time")
                .HasColumnType("timestamp");
            
            
            builder.Property(s => s.SessionStatus)
                .HasColumnName("status")
                .HasConversion<int>()
                .IsRequired();
            
            
            builder.HasOne(s => s.Movie)
                .WithMany(m => m.Sessions)
                .HasForeignKey(s => s.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            
            builder.HasOne(s => s.Hall)
                .WithMany(h => h.Sessions)
                .HasForeignKey(s => s.HallId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}