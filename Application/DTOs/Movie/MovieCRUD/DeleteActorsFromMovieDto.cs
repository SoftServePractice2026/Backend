namespace Application.Dtos.Movie;

public record DeleteActorsFromMovieDto(Guid MovieId, List<Guid> ActorIds);