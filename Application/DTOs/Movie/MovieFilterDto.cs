using Application.DTOs.Common;
using Application.Interfaces;
using Shared.Common;

namespace Application.Dtos.Movie;

public record MovieFilterDto(
    string? Title, 
    List<Guid>? GenreIds, 
    int? MinAgeRating,
    string? OrderBy = null,
    SortDirection SortDirection = SortDirection.Ascending)
    : PaginationBaseDto, ISortable;