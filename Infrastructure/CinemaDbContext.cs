using Domain.Entities;
using Infrastructure.EntitiesConfiguration;
using Infrastructure.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class CinemaDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{

    public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options)
    { }

    public DbSet<ActorEntity> Actors { get; set; }
    
    public DbSet<FavoriteMovieEntity> FavoriteMovies { get; set; }
    
    public DbSet<GenreEntity> Genres { get; set; }
    
    public DbSet<HallEntity> Halls { get; set; }
    
    public DbSet<MovieActorEntity> MovieActors { get; set; }
    
    public DbSet<MovieEntity> Movies { get; set; }
    
    public DbSet<PaymentTransactionEntity> PaymentTransactions { get; set; }
    
    public DbSet<SeatEntity> Seats { get; set; }
    
    public DbSet<SessionEntity> Sessions { get; set; }
    
    public DbSet<TicketEntity> Tickets { get; set; }
    
    public DbSet<ViewHistoryEntity> ViewHistories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CinemaDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}