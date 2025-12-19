using Domain.ValueObjects;

namespace Domain.Entities
{
    public class GitHubRepository
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public GitHubUrl HtmlUrl { get; set; } = null!;
        public RepositoryStats Stats { get; set; } = null!; // Agrupamento de stargazers_count, forks_count, etc
        public bool IsFavorite { get; set; }
    }
}
