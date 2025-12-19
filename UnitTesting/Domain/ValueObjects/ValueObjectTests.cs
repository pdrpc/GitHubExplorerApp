using FluentAssertions;
using Xunit;
using Domain.ValueObjects;
using System;

namespace GitHubExplorerApp.Tests.Domain.ValueObjects
{
    public class ValueObjectTests
    {
        [Fact]
        public void GitHubUrl_DeveCriarInstancia_QuandoUrlForValida()
        {
            // Arrange
            var validUrl = "https://github.com/usuario/repo";

            // Act
            var gitHubUrl = new GitHubUrl(validUrl);

            // Assert
            gitHubUrl.Value.Should().Be(validUrl);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("https://google.com")] // Não contém github.com
        public void GitHubUrl_DeveLancarExcecao_QuandoUrlForInvalida(string invalidUrl)
        {
            // Act
            Action act = () => new GitHubUrl(invalidUrl);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("URL do GitHub inválida.");
        }

        [Fact]
        public void RepositoryStats_DeveZerarValores_QuandoForemNegativos()
        {
            // Arrange & Act
            var stats = new RepositoryStats(-10, -5, -1);

            // Assert
            stats.Stars.Should().Be(0);
            stats.Forks.Should().Be(0);
            stats.Watchers.Should().Be(0);
        }

        [Fact]
        public void ValueObjects_Iguais_DeveRetornarTrueNoEquals()
        {
            // Teste da lógica herdada de ValueObject
            var stats1 = new RepositoryStats(10, 10, 10);
            var stats2 = new RepositoryStats(10, 10, 10);

            stats1.Equals(stats2).Should().BeTrue();
        }
    }
}