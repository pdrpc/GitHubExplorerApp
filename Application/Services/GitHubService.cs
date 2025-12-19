using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class GitHubAppService : IGitHubAppService
    {
        private readonly IGithubService _githubIntegration;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IRelevanceService _relevanceService;

        public GitHubAppService(IGithubService githubIntegration, IFavoriteRepository favoriteRepository, IRelevanceService relevanceService)
        {
            _githubIntegration = githubIntegration ?? throw new ArgumentNullException(nameof(githubIntegration));
            _favoriteRepository = favoriteRepository ?? throw new ArgumentNullException(nameof(favoriteRepository));
            _relevanceService = relevanceService ?? throw new ArgumentNullException(nameof(relevanceService));
        }

        
        // Busca repositórios, calcula a relevância e ordena os resultados
        public async Task<IEnumerable<GitHubRepositoryResponse>> SearchAndSortAsync(string query)
        {
            var repositories = await _githubIntegration.SearchRepositoriesAsync(query);
            var favorites = await _favoriteRepository.GetAllAsync();
            var favoriteIds = favorites.Select(f => f.Id).ToHashSet();

            return repositories.Select(repo =>
            {
                var isFavorite = favoriteIds.Contains(repo.Id);
                var score = _relevanceService.CalculateRelevance(
                    repo.Stats.Stars,
                    repo.Stats.Forks,
                    repo.Stats.Watchers);

                return MapToResponse(repo, isFavorite, score);
            })
            .OrderByDescending(r => r.RelevanceScore).ToList();
        }

        
        // Retorna todos os repositórios marcados como favoritos
        public async Task<IEnumerable<GitHubRepositoryResponse>> GetFavoritesAsync()
        {
            var favorites = await _favoriteRepository.GetAllAsync();

            return favorites.Select(repo =>
            {
                var score = _relevanceService.CalculateRelevance(repo.Stats.Stars, repo.Stats.Forks, repo.Stats.Watchers);
                return MapToResponse(repo, true, score);
            });
        }

        
        // Adiciona um repositório aos favoritos em tempo de execução
        public async Task AddFavoriteAsync(GitHubRepositoryResponse dto)
        {
            var entity = new GitHubRepository
            {
                Id = dto.Id,
                Name = dto.Name,
                FullName = dto.FullName,
                Description = dto.Description,
                HtmlUrl = new Domain.ValueObjects.GitHubUrl(dto.Url),
                Stats = new Domain.ValueObjects.RepositoryStats(dto.Stars, dto.Forks, dto.Watchers),
                IsFavorite = true
            };

            await _favoriteRepository.AddAsync(entity);
        }

        
        // Remove um repositório dos favoritos
        public async Task RemoveFavoriteAsync(string repositoryId)
        {
            await _favoriteRepository.RemoveAsync(repositoryId);
        }

        // Helper privado para mapeamento de Entidade para DTO 
        private static GitHubRepositoryResponse MapToResponse(GitHubRepository repo, bool isFavorite, double score)
        {
            return new GitHubRepositoryResponse(
                repo.Id,
                repo.Name,
                repo.FullName,
                repo.Description,
                repo.HtmlUrl.Value,
                repo.Stats.Stars,
                repo.Stats.Forks,
                repo.Stats.Watchers,
                score,
                isFavorite
            );
        }
    }
}
