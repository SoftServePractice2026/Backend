namespace Application.Dtos.Movie;

public record AddActorsToMovieDto(Guid MovieId, List<Guid> ActorIds);