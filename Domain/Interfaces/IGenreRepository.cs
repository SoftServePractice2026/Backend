using Domain.Entities;
using Domain.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IGenreRepository
    {
        void CreateGenre(GenreEntity genreEntity);
        void DeleteGenre(GenreEntity genreEntity);
        void UpdateGenre(GenreEntity genreEntity);
        Task<GenreEntity?> GetGenreByIdAsync(Guid genreId, CancellationToken cancellationToken);
        Task<GenreEntity?> GetGenreByNameAsync(string name, CancellationToken cancellationToken);
        Task<List<GenreEntity>> GetAllGenresAsync(CancellationToken cancellationToken);

    }
}
