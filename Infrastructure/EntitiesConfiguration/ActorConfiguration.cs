using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.EntitiesConfiguration
{
    public class ActorConfiguration : IEntityTypeConfiguration<ActorEntity>
    {
        public void Configure(EntityTypeBuilder<ActorEntity> builder)
        {
            builder.ToTable("actor");
            
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("actor_id");
            
            builder.Property(a => a.FirstName)
                .HasColumnName("first_name").HasMaxLength(30)
                .IsRequired();
            
            builder.Property(a => a.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(40).IsRequired();;
        }
    }
}