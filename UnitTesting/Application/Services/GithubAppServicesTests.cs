using Moq;
using Xunit;
using FluentAssertions;
using Application.Services;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using Application.Interfaces;


namespace GitHubExplorerApp.Tests.Application.Services
{
    public class GithubAppServiceTests
    {
        private readonly Mock<IGithubService> _githubServiceMock;
        private readonly Mock<IFavoriteRepository> _favoriteRepoMock;
        private readonly Mock<IRelevanceService> _relevanceServiceMock;
        private readonly GitHubAppService _appService;

        public GithubAppServiceTests()
        {
            _githubServiceMock = new Mock<IGithubService>();
            _favoriteRepoMock = new Mock<IFavoriteRepository>();
            _relevanceServiceMock = new Mock<IRelevanceService>();

            _appService = new GitHubAppService(
                _githubServiceMock.Object,
                _favoriteRepoMock.Object,
                _relevanceServiceMock.Object
            );
        }

        [Fact]
        public async Task SearchAndSortAsync_DeveRetornarRepositoriosOrdenadosESinalizarFavoritos()
        {
            // Arrange
            var query = "dotnet";
            var repoIdFavorito = "1";

            // Simula retorno da API do Github
            var githubRepos = new List<GitHubRepository>
            {
                new GitHubRepository
                {
                    Id = "1",
                    Name = "Repo A",
                    Stats = new RepositoryStats(10, 0, 0),
                    // ADICIONE ESTA LINHA:
                    HtmlUrl = new GitHubUrl("https://github.com/teste/repo-a")
                },
                new GitHubRepository
                {
                    Id = "2",
                    Name = "Repo B",
                    Stats = new RepositoryStats(50, 0, 0),
                    // ADICIONE ESTA LINHA:
                    HtmlUrl = new GitHubUrl("https://github.com/teste/repo-b")
                }
            };

            // Simula que o ID "1" já está nos favoritos do banco
            var favorites = new List<GitHubRepository>
            {
                new GitHubRepository { Id = repoIdFavorito }
            };

            _githubServiceMock.Setup(x => x.SearchRepositoriesAsync(query))
                .ReturnsAsync(githubRepos);

            _favoriteRepoMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(favorites);

            // Simula cálculos de relevância diferentes para testar a ordenação
            _relevanceServiceMock.Setup(x => x.CalculateRelevance(10, 0, 0)).Returns(10.0);
            _relevanceServiceMock.Setup(x => x.CalculateRelevance(50, 0, 0)).Returns(50.0);

            // Act
            var result = await _appService.SearchAndSortAsync(query);

            // Assert
            var resultList = result.ToList();
            resultList.Should().HaveCount(2);

            // Verifica se o Repo B (maior score) veio primeiro (OrderByDescending)
            resultList[0].Name.Should().Be("Repo B");
            resultList[0].RelevanceScore.Should().Be(50.0);
            resultList[0].IsFavorite.Should().BeFalse();

            // Verifica se o Repo A foi marcado corretamente como favorito
            var repoA = resultList.Find(r => r.Id == "1");
            repoA.Should().NotBeNull();
            repoA!.IsFavorite.Should().BeTrue();
        }

        [Fact]
        public async Task GetFavoritesAsync_DeveRetornarApenasFavoritosComScoreCalculado()
        {
            // Arrange
            var favorites = new List<GitHubRepository>
            {
                new GitHubRepository {
                    Id = "1",
                    Name = "Fav A",
                    Stats = new RepositoryStats(10, 10, 10),
                    HtmlUrl = new GitHubUrl("https://github.com/a")
                }
            };

            _favoriteRepoMock.Setup(x => x.GetAllAsync()).ReturnsAsync(favorites);
            _relevanceServiceMock.Setup(x => x.CalculateRelevance(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(100.0);

            // Act
            var result = await _appService.GetFavoritesAsync();

            // Assert
            var fav = result.First();
            fav.IsFavorite.Should().BeTrue();
            fav.RelevanceScore.Should().Be(100.0);
        }

        [Fact]
        public async Task AddFavoriteAsync_DeveMapearDTOParaEntidadeEConservarStatus()
        {
            // Arrange
            var dto = new GitHubRepositoryResponse("1", "Nome", "Full", "Desc", "https://github.com/a", 10, 5, 2, 0, false);

            // Act
            await _appService.AddFavoriteAsync(dto);

            // Assert
            // Verifica se o repositório foi chamado com uma entidade que tem IsFavorite = true
            _favoriteRepoMock.Verify(x => x.AddAsync(It.Is<GitHubRepository>(r =>
                r.Id == dto.Id &&
                r.IsFavorite == true)), Times.Once);
        }

        [Fact]
        public async Task RemoveFavoriteAsync_DeveChamarRepositorioComIdCorreto()
        {
            // Arrange
            var id = "999";

            // Act
            await _appService.RemoveFavoriteAsync(id);

            // Assert
            _favoriteRepoMock.Verify(x => x.RemoveAsync(id), Times.Once);
        }
    }
}