using Application.Services.ExternalMovie;
using Application.DTOs.ExternalMovieDto;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;

namespace SoftServePractice2026.Backend.Tests.Services
{
    public class TMDBServiceTests
    {
        private TMDBService CreateService(string jsonResponse)
        {
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://api.themoviedb.org/3/")
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Tmdb:ApiKey"] = "fake-key",
                    ["Tmdb:BaseUrl"] = "https://api.themoviedb.org/3/"
                })
                .Build();

            return new TMDBService(httpClient, config);
        }

        [Fact]
        public async Task GetMovieAsync_ShouldReturnMovie_WhenResponseIsValid()
        {
            
            var json = """
            {
              "id": 1,
              "title": "Test Movie",
              "overview": "Description",
              "poster_path": "/poster.jpg",
              "runtime": 120,
              "original_language": "en",
              "vote_average": 8.5,
              "adult": false,
              "release_date": "2023-01-01",
              "genres": [
                { "name": "Drama" }
              ],
              "credits": {
                "cast": [
                  {
                    "id": 10,
                    "name": "Actor Name",
                    "profile_path": "/actor.jpg",
                    "character": "Hero"
                  }
                ]
              }
            }
            """;

            var service = CreateService(json);

            
            var result = await service.GetMovieAsync(1);

            
            result.Should().NotBeNull();
            result!.Title.Should().Be("Test Movie");
            result.Rating.Should().Be(8.5m);
            result.Genres.Should().Contain("Drama");
            result.Cast.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetPopularMovieIdsAsync_ShouldReturnIds()
        {
            
            var json = """
            {
              "results": [
                { "id": 1 },
                { "id": 2 },
                { "id": 3 }
              ]
            }
            """;

            var service = CreateService(json);

           
            var result = await service.GetPopularMovieIdsAsync(1);

            
            result.Should().Contain(new[] { 1, 2, 3 });
        }
    }
}
