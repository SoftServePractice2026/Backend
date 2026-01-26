using Shared.Common;

namespace Application.Interfaces
{
    public interface ISortable
    {
        string? OrderBy { get; }
        SortDirection SortDirection { get; }
    }
}
