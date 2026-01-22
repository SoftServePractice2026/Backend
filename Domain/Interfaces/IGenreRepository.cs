using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IGenreRepository
    {
        Task CreateGenreAsync(GenreEntity genreEntity);
        Task DeleteGenreAsync(GenreEntity genreEntity);
        Task UpdateGenreAsync(GenreEntity genreEntity);
        Task<GenreEntity?> GetGenreByIdAsync(Guid genreId);
        Task<GenreEntity> GetGenreByNameAsync(string name);
        Task<IReadOnlyList<GenreEntity>> GetGenreEntitiesAsync();
        Task SaveChangesAsync();
    }
}
