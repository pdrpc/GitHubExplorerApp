using FluentAssertions;
using Xunit;
using Infrastructure.Persistance;
using Domain.Entities;

namespace GitHubExplorerApp.Tests.Infrastructure.Persistence
{
    public class InMemoryFavoriteRepositoryTests
    {
        private readonly InMemoryFavoriteRepository _repository;

        public InMemoryFavoriteRepositoryTests()
        {
            _repository = new InMemoryFavoriteRepository();
        }

        [Fact]
        public async Task AddAsync_DeveAdicionarRepositórioComSucesso()
        {
            // Arrange
            var repo = new GitHubRepository { Id = "1", Name = "Teste" };

            // Act
            await _repository.AddAsync(repo);
            var all = await _repository.GetAllAsync();

            // Assert
            all.Should().ContainSingle(r => r.Id == "1");
        }

        [Fact]
        public async Task RemoveAsync_DeveRemoverRepositórioExistente()
        {
            // Arrange
            var repo = new GitHubRepository { Id = "1", Name = "Teste" };
            await _repository.AddAsync(repo);

            // Act
            await _repository.RemoveAsync("1");
            var all = await _repository.GetAllAsync();

            // Assert
            all.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarTodosOsItensAdicionados()
        {
            // Arrange
            await _repository.AddAsync(new GitHubRepository { Id = "1" });
            await _repository.AddAsync(new GitHubRepository { Id = "2" });

            // Act
            var all = await _repository.GetAllAsync();

            // Assert
            all.Should().HaveCount(2);
        }
    }
}