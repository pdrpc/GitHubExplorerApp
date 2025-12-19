using FluentAssertions;
using Xunit;
using Domain.Services;

namespace GitHubExplorerApp.Tests.Domain.Services
{
    public class RelevanceServiceTests
    {
        private readonly RelevanceService _service;

        public RelevanceServiceTests()
        {
            _service = new RelevanceService();
        }

        [Theory]
        [InlineData(10, 5, 4, 44.0)] // (10*3) + (5*2) + 4 = 30 + 10 + 4 = 44
        [InlineData(1, 1, 1, 6.0)]   // (1*3) + (1*2) + 1 = 6
        [InlineData(0, 0, 10, 10.0)] // Apenas watchers
        public void CalculateRelevance_DeveSeguirAFormulaCorreta(int stars, int forks, int watchers, double esperado)
        {
            // Act
            var result = _service.CalculateRelevance(stars, forks, watchers);

            // Assert
            result.Should().Be(esperado);
        }
    }
}