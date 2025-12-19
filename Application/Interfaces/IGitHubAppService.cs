using Application.DTOs;

namespace Application.Interfaces
{
    public interface IGitHubAppService
    {
        Task<IEnumerable<GitHubRepositoryResponse>> SearchAndSortAsync(string query);
        Task<IEnumerable<GitHubRepositoryResponse>> GetFavoritesAsync();
        Task AddFavoriteAsync(GitHubRepositoryResponse repositoryDto);
        Task RemoveFavoriteAsync(string repositoryId);
    }
}
