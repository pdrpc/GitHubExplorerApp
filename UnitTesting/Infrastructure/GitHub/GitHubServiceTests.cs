using Moq;
using Xunit;
using FluentAssertions;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using Infrastructure.GitHub.Services;
using Infrastructure.GitHub.Models;

namespace GitHubExplorerApp.Tests.Infrastructure.GitHub
{
    public class GitHubServiceTests
    {
        [Fact]
        public async Task SearchRepositoriesAsync_DeveRetornarListaMapeada_QuandoApiResponderSucesso()
        {
            // Arrange
            var query = "dotnet";
            var mockResponse = new GitHubSearchResponse(new List<GitHubRepoItem>
            {
                new GitHubRepoItem(123, "Repo-Teste", "Usuario/Repo-Teste", "Descricao", "https://github.com/url", 10, 5, 2)
            });

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(mockResponse))
                });

            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("https://api.github.com/") };
            var service = new GitHubService(httpClient);

            // Act
            var result = await service.SearchRepositoriesAsync(query);

            // Assert
            result.Should().NotBeEmpty();
            var repo = result.First();
            repo.Id.Should().Be("123");
            repo.Name.Should().Be("Repo-Teste");
            repo.Stats.Stars.Should().Be(10);
        }

        [Fact]
        public async Task SearchRepositoriesAsync_DeveRetornarVazio_QuandoItemsForNull()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("{}") });

            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("https://api.github.com/") };
            var service = new GitHubService(httpClient);

            // Act
            var result = await service.SearchRepositoriesAsync("query");

            // Assert
            result.Should().BeEmpty();
        }
    }
}