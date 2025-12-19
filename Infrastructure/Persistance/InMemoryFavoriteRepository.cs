using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistance
{
    public class InMemoryFavoriteRepository : IFavoriteRepository
    {
        // Usamos ConcurrentDictionary para garantir thread-safety em tempo de execução 
        private readonly ConcurrentDictionary<string, GitHubRepository> _favorites = new();

        public Task AddAsync(GitHubRepository repository)
        {
            _favorites.TryAdd(repository.Id, repository);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string repositoryId)
        {
            _favorites.TryRemove(repositoryId, out _);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<GitHubRepository>> GetAllAsync()
        {
            return Task.FromResult(_favorites.Values.AsEnumerable());
        }
    }
}
