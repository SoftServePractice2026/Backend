namespace Application.DTOs.Common
{
    public record PaginationBaseDto(
        int? PageNumber = 1,
        int? PageSize = 10);
}
