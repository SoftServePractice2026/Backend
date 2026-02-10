namespace Application.Dtos.Identity;

public class AddFavoriteMovieRequest
{
    public Guid MovieId { get; set; }
}

public class FavoriteMovieResponse
{
    public Guid FavoriteId { get; set; }
    public Guid MovieId { get; set; }
    public Guid UserId { get; set; }
    public DateTime AddedAt { get; set; }
}
