using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public record GitHubRepositoryResponse
    (
        string Id,
        string Name,
        string FullName,
        string Description,
        string Url,
        int Stars,
        int Forks,
        int Watchers,
        double RelevanceScore, // Pontuação calculada
        bool IsFavorite
    );
}
