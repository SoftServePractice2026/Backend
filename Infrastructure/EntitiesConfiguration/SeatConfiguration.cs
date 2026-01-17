using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;


namespace Infrastructure.EntitiesConfiguration
{
    public class SeatConfiguration : IEntityTypeConfiguration<SeatEntity>
    {
        public void Configure(EntityTypeBuilder<SeatEntity> builder)
        {
            builder.ToTable("seat");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .HasColumnName("seat_id");
            
            builder.Property(s => s.HallId)
                .HasColumnName("hall_id")
                .IsRequired();

            builder.Property(s => s.RowNumber)
                .HasColumnName("row_number")
                .IsRequired();
            
            builder.Property(s => s.SeatNumber)
                .HasColumnName("seat_number")
                .IsRequired();

            builder.Property(s => s.SeatType)
                .HasColumnName("seat_type")
                .HasConversion<int>()
                .IsRequired();
            
            builder.Property(s => s.SeatStatus)
                .HasColumnName("status")
                .HasConversion<int>()
                .IsRequired();
            
            builder.HasIndex(s => new { s.HallId, s.RowNumber, s.SeatNumber })
                .IsUnique();

            builder.HasOne(s => s.Hall)
                .WithMany(h => h.Seats)
                .HasForeignKey(s => s.HallId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}