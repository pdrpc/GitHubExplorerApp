using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IGithubService
    {
        Task<IEnumerable<GitHubRepository>> SearchRepositoriesAsync(string query);
    }
}
