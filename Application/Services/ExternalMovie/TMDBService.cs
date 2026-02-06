using Application.DTOs.ExternalMovieDto;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Application.Services.ExternalMovie
{
    public class TMDBService : IExternalMovieService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _imageBaseUrl = "https://image.tmdb.org/t/p/w500"; 

        public TMDBService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration["Tmdb:BaseUrl"] ?? "https://api.themoviedb.org/3/");
            _apiKey = configuration["Tmdb:ApiKey"]!;
        }

        public async Task<ExternalMovieDto?> GetMovieAsync(int tmdbId)
        {
            var query = $"movie/{tmdbId}?api_key={_apiKey}&language=uk-UA&append_to_response=credits";

            try
            {
                var tmdbResponse = await _httpClient.GetFromJsonAsync<TmdbMovieResponse>(query);

                if (tmdbResponse == null) return null;

                DateTime releaseDate;
                DateTime.TryParse(tmdbResponse.ReleaseDateString, out releaseDate);
                if (releaseDate == default) releaseDate = DateTime.UtcNow;

                return new ExternalMovieDto(
                    TmdbId: tmdbResponse.Id,
                    Title: tmdbResponse.Title ?? "No Title",
                    Description: tmdbResponse.Overview ?? "",
                    Poster: !string.IsNullOrEmpty(tmdbResponse.PosterPath) ? _imageBaseUrl + tmdbResponse.PosterPath : null!,
                    Duration: tmdbResponse.Runtime ?? 0,
                    Language: tmdbResponse.Language ?? "en",
                    Rating: tmdbResponse.VoteAverage,
                    AgeRating: tmdbResponse.IsAdult,

                    Genres: tmdbResponse.Genres?.Select(g => g.Name).ToList() ?? new List<string>(),

                    Cast: tmdbResponse.Credits?.Cast?.Take(10).Select(c => new ExternalActorDto(
                        TmdbId: c.Id,
                        FullName: c.Name,
                        PhotoUrl: !string.IsNullOrEmpty(c.ProfilePath) ? _imageBaseUrl + c.ProfilePath : null!,
                        Character: c.Character!
                    )).ToList() ?? new List<ExternalActorDto>(),

                    ReleaseDate: releaseDate
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching movie {tmdbId}: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<int>> GetPopularMovieIdsAsync(int page)
        {
            var query = $"movie/popular?api_key={_apiKey}&language=uk-UA&page={page}";
            try
            {
                var response = await _httpClient.GetFromJsonAsync<TmdbIdResponse>(query);
                return response?.Results.Select(x => x.Id) ?? Enumerable.Empty<int>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching popular list: {ex.Message}");
                return Enumerable.Empty<int>();
            }
        }

        internal class TmdbIdResponse
        {
            public List<TmdbShortItem> Results { get; set; } = new();
        }

        internal class TmdbShortItem
        {
            public int Id { get; set; }
        }

    }
}