using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PresentationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GitHubController : ControllerBase
    {
        private readonly IGitHubAppService _appService;

        public GitHubController(IGitHubAppService appService)
        {
            _appService = appService;
        }

        [HttpGet("repos")]
        public async Task<ActionResult<IEnumerable<GitHubRepositoryResponse>>> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("O termo de busca é obrigatório.");

            var result = await _appService.SearchAndSortAsync(query);
            return Ok(result);
        }

        [HttpGet("favoritos")]
        public async Task<ActionResult<IEnumerable<GitHubRepositoryResponse>>> GetFavorites()
        {
            var result = await _appService.GetFavoritesAsync();
            return Ok(result);
        }

        [HttpPost("favoritos")]
        public async Task<IActionResult> AddFavorite([FromBody] GitHubRepositoryResponse repo)
        {
            await _appService.AddFavoriteAsync(repo);
            return Ok();
        }

        [HttpDelete("favoritos/{id}")]
        public async Task<IActionResult> RemoveFavorite(string id)
        {
            await _appService.RemoveFavoriteAsync(id);
            return NoContent();
        }
    }
}
