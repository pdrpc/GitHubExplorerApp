using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.GitHub.Models
{
    internal record GitHubSearchResponse(
        [property: JsonPropertyName("items")] List<GitHubRepoItem> Items
    );

    internal record GitHubRepoItem(
        [property: JsonPropertyName("id")] long Id,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("full_name")] string FullName,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("html_url")] string HtmlUrl,
        [property: JsonPropertyName("stargazers_count")] int Stars,
        [property: JsonPropertyName("forks_count")] int Forks,
        [property: JsonPropertyName("watchers_count")] int Watchers
    );
}
