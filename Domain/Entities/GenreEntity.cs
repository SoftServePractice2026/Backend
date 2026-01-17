using Domain.Primitives;

namespace Domain.Entities
{
    public class GenreEntity : Entity
    {
        public string Name { get; set; } = null!;

        public ICollection<MovieEntity> Movies { get; set; } = new List<MovieEntity>();
    }
}
