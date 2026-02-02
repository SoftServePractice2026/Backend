using Application.DTOs.ExternalMovieDto;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;

namespace Application.Services.ExternalMovie
{
    public class TMDBService : IExternalMovieService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public TMDBService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(configuration["Tmdb:BaseUrl"]
                                      ?? "https://api.themoviedb.org/3/");

            _apiKey = configuration["Tmdb:ApiKey"];
        }

        public async Task<ExternalMovieDto?> GetMovieAsync(int tmdbId)
        {
            var query = $"movie/{tmdbId}?api_key={_apiKey}&language=uk-UA&append_to_response=credits";

            try
            {
                return await _httpClient.GetFromJsonAsync<ExternalMovieDto>(query);
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<int>> GetPopularMovieIdsAsync(int page)
        {
            var query = $"movie/popular?api_key={_apiKey}&language=uk-UA&page={page}";

            try
            {
                var response = await _httpClient.GetFromJsonAsync<TmdbIdResponse>(query);

                if (response?.Results != null)
                {
                    return response.Results.Select(x => x.Id);
                }

                return Enumerable.Empty<int>();
            }
            catch (Exception)
            {
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