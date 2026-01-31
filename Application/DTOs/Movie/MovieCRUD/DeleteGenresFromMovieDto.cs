namespace Application.Dtos.Movie;

public record DeleteGenresFromMovieDto(Guid MovieId, List<Guid> GenreIds);