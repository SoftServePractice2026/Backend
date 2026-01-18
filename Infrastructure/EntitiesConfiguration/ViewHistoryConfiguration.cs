using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using Infrastructure.Identity.Data;

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

            builder.Property(h => h.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(h => h.SessionId)
                .HasColumnName("session_id")
                .IsRequired();
            
            builder.Property(h => h.ViewedAt)
                .HasColumnName("viewed_at")
                .HasColumnType("timestamp")
                .IsRequired();
            
            builder.HasOne(h => h.Session)
                .WithMany(s => s.ViewHistories)
                .HasForeignKey(h => h.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(h => (ApplicationUser)h.ApplicationUser)
                .WithMany(u => u.ViewHistories)
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}