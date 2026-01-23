using Domain.Entities;
using Domain.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Data
{
    public class ApplicationUser : IdentityUser<Guid>, IUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime BirthDate { get; set; }

        public string? RefreshToken { get; set; }
        
        public DateTime RefreshTokenExpiryTime { get; set; }
        
        public ICollection<ViewHistoryEntity> ViewHistories { get; set; } = new List<ViewHistoryEntity>();
        public ICollection<FavoriteMovieEntity> FavoriteMovies { get; set; } = new List<FavoriteMovieEntity>();
        public ICollection<TicketEntity> Tickets { get; set; } = new List<TicketEntity>();
        public ICollection<PaymentTransactionEntity> PaymentTransactions { get; set; } = new List<PaymentTransactionEntity>();
    }
}
