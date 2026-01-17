namespace Domain.Primitives
{
    public interface ITimeModification
    {
        DateTime CreatedAt { get; set; }
        DateTime? LastModifiedAt { get; set; }
    }
}
