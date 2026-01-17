namespace Domain.Entities.Interfaces
{
    public interface IUser
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        public ICollection<ViewHistoryEntity> ViewHistories { get; set; }
        public ICollection<FavoriteMovieEntity> FavoriteMovies { get; set; }
        public ICollection<TicketEntity> Tickets { get; set; }
        public ICollection<PaymentTransactionEntity> PaymentTransactions { get; set; }
    }
}
