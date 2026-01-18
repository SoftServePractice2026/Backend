using Domain.Entities;
using Infrastructure.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntitiesConfiguration
{
    public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransactionEntity>
    {
        public void Configure(EntityTypeBuilder<PaymentTransactionEntity> builder)
        {
            builder.ToTable("payment_transaction");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnName("payment_transaction_id");

            builder.Property(p => p.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(p => p.Currency)
                .HasColumnName("currency")
                .HasMaxLength(5)
                .IsRequired();

            builder.Property(p => p.PaymentMethod)
               .HasColumnName("payment_method")
               .HasConversion<int>()
               .IsRequired();

            builder.Property(p => p.PaymentStatus)
               .HasColumnName("payment_status")
               .HasConversion<int>()
               .IsRequired();

            builder.Property(p => p.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("timezone('utc', now())")
                .IsRequired();

            builder.Property(p => p.LastModifiedAt)
                .HasColumnName("last_modified_at")
                .HasColumnType("timestamp");

            builder.HasOne(p => (ApplicationUser)p.ApplicationUser)
                .WithMany(u => u.PaymentTransactions)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
