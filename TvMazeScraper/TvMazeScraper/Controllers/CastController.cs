using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TvMazeScraper.Models;
using TvMazeScraper.Services.Interfaces;
using TvMazeScraper.Utils.Interfaces;

namespace TvMazeScraper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CastController : Controller
    {
        private readonly IShowReadService _showReadService;
        private readonly IApiSettingsProvider _settingsProvider;

        public CastController(IShowReadService showReadService, IApiSettingsProvider settingsProvider)
        {
            _showReadService = showReadService;
            _settingsProvider = settingsProvider;
        }

        /// <summary>
        /// Cast data from TvMaze scraper by  show id
        /// </summary>
        [HttpGet("show/{showId}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Cast>>> Get([Required]int showId, [FromQuery]int? page, [FromQuery]int? pageSize)
        {
            if (showId <= 0 || page.GetValueOrDefault() < 0 || pageSize.GetValueOrDefault() < 0)
            {
                return BadRequest();
            }

            if (!page.HasValue)
            {
                page = 1;
            }

            if (!pageSize.HasValue)
            {
                pageSize = _settingsProvider.DefaultPageSize;
            }

            var result = await _showReadService.GetCastAsync(showId, page.Value, pageSize.Value);
            if (result == null || !result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Cast data from TvMaze scraper by  show id
        /// </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Cast>>> Get([FromQuery]int? id, [FromQuery]string name, [FromQuery]int? page, [FromQuery]int? pageSize)
        {
            if ((id.HasValue && id.Value < 0) || page.GetValueOrDefault() < 0 || pageSize.GetValueOrDefault() < 0)
            {
                return BadRequest();
            }

            if (id == 0 && string.IsNullOrWhiteSpace(name))
            {
                return BadRequest();
            }

            if (!page.HasValue)
            {
                page = 1;
            }

            if (!pageSize.HasValue)
            {
                pageSize = _settingsProvider.DefaultPageSize;
            }

            var result = await _showReadService.GetCastAsync(id, name, page.Value, pageSize.Value);

            if (result == null || !result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
