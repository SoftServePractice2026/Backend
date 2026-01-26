using Application.DTOs.Genre;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Shared;
namespace Application.Services.Genre
{
    public interface IGenreService
    {
        Task<Result<GenreDetailsDto>> CreateGenreAsync(GenreCreateDto dto, CancellationToken cancellationToken);
        Task<Result<GenreDetailsDto>> UpdateGenreAsync(Guid targetId, GenreUpdateDto dto, CancellationToken cancellationToken);
        Task<Result<bool>> DeleteGenreAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<GenreDetailsDto>> GetGenreByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<GenreDetailsDto>> GetGenreByNameAsync(string name, CancellationToken cancellationToken);
        Task<Result<List<GenreListItemDto>>> GetGenreAllAsync(CancellationToken cancellationToken);
    }

}
