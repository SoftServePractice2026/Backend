namespace Application.Dtos.Movie;

public record AddGenresToMovieDto(Guid MovieId, List<Guid> GenreIds);