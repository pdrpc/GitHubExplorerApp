using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PresentationApi.Controllers;
using Application.Interfaces;
using Application.DTOs;

namespace GitHubExplorerApp.Tests.Presentation.Controllers
{
    public class GitHubControllerTests
    {
        private readonly Mock<IGitHubAppService> _appServiceMock;
        private readonly GitHubController _controller;

        public GitHubControllerTests()
        {
            _appServiceMock = new Mock<IGitHubAppService>();
            _controller = new GitHubController(_appServiceMock.Object);
        }

        [Fact]
        public async Task Search_DeveRetornarBadRequest_QuandoQueryForVazia()
        {
            // Act
            var result = await _controller.Search("");

            // Assert 
            var badRequest = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequest.Value.Should().Be("O termo de busca é obrigatório.");
        }

        [Fact]
        public async Task Search_DeveRetornarOk_ComListaDeRepositorios()
        {
            // Arrange
            var query = "dotnet";
            var mockResponse = new List<GitHubRepositoryResponse>
            {
                new GitHubRepositoryResponse("1", "Repo", "Full", "Desc", "url", 10, 5, 2, 44.0, false)
            };

            _appServiceMock.Setup(x => x.SearchAndSortAsync(query))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.Search(query);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(mockResponse);
            _appServiceMock.Verify(x => x.SearchAndSortAsync(query), Times.Once);
        }

        [Fact]
        public async Task GetFavorites_DeveRetornarOk_ComListaDeFavoritos()
        {
            // Arrange
            var mockFavs = new List<GitHubRepositoryResponse>
            {
                new GitHubRepositoryResponse("1", "Fav", "Full", "Desc", "url", 10, 5, 2, 44.0, true)
            };

            _appServiceMock.Setup(x => x.GetFavoritesAsync()).ReturnsAsync(mockFavs);

            // Act
            var result = await _controller.GetFavorites();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(mockFavs);
        }

        [Fact]
        public async Task AddFavorite_DeveRetornarOk_AoAdicionarComSucesso()
        {
            // Arrange
            var dto = new GitHubRepositoryResponse("1", "Repo", "Full", "Desc", "url", 10, 5, 2, 44.0, false);

            // Act
            var result = await _controller.AddFavorite(dto);

            // Assert 
            result.Should().BeOfType<OkResult>();
            _appServiceMock.Verify(x => x.AddFavoriteAsync(dto), Times.Once);
        }

        [Fact]
        public async Task RemoveFavorite_DeveRetornarNoContent_AoRemoverComSucesso()
        {
            // Arrange
            var id = "123";

            // Act
            var result = await _controller.RemoveFavorite(id);

            // Assert 
            result.Should().BeOfType<NoContentResult>();
            _appServiceMock.Verify(x => x.RemoveFavoriteAsync(id), Times.Once);
        }
    }
}