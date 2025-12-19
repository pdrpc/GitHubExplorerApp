using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using Infrastructure.GitHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.GitHub.Services
{
    public class GitHubService : IGithubService
    {
        private readonly HttpClient _httpClient;

        public GitHubService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<GitHubRepository>> SearchRepositoriesAsync(string query)
        {
            // Implementação da busca conforme requisito 1
            var response = await _httpClient.GetFromJsonAsync<GitHubSearchResponse>($"search/repositories?q={query}");

            if (response?.Items == null) return Enumerable.Empty<GitHubRepository>();

            return response.Items.Select(item => new GitHubRepository
            {
                Id = item.Id.ToString(),
                Name = item.Name,
                FullName = item.FullName,
                Description = item.Description ?? string.Empty,
                HtmlUrl = new GitHubUrl(item.HtmlUrl),
                Stats = new RepositoryStats(item.Stars, item.Forks, item.Watchers)
            });
        }
    }
}
