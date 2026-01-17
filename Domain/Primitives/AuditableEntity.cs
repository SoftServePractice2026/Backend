namespace Domain.Primitives
{
    public class AuditableEntity : Entity, ITimeModification
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}
