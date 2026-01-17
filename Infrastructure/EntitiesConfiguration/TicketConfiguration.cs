using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.EntitiesConfiguration
{
    public class TicketConfiguration : IEntityTypeConfiguration<TicketEntity>
    {
        public void Configure(EntityTypeBuilder<TicketEntity> builder)
        {
            builder.ToTable("ticket");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
                .HasColumnName("ticket_id");

            builder.Property(t => t.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(t => t.SessionId)
                .HasColumnName("session_id")
                .IsRequired();
            
            builder.Property(t => t.SeatId)
                .HasColumnName("seat_id")
                .IsRequired();
            
            builder.Property(t => t.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp")
                .IsRequired(false);
            
            builder.Property(t => t.Price)
                .HasColumnName("price")
                .HasColumnType("numeric(10,2)")
                .IsRequired();
            
            builder.HasIndex(t => new { t.SessionId, t.SeatId })
                .IsUnique();
            
            builder.HasOne(t => t.Session)
                .WithMany(s => s.Tickets)
                .HasForeignKey(t => t.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.ApplicationUser)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.PaymentTransaction)
                .WithMany(p => p.Tickets)
                .HasForeignKey(t => t.PaymentTransactionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Seat)
                .WithMany(s => s.Tickets)
                .HasForeignKey(t => t.SeatId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}