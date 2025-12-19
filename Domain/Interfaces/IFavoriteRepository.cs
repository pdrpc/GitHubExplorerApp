using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IFavoriteRepository
    {
        Task AddAsync(GitHubRepository repository);
        Task RemoveAsync(string repositoryId);
        Task<IEnumerable<GitHubRepository>> GetAllAsync();
    }
}
