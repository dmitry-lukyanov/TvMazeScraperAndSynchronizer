using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
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
    public class ShowController : Controller
    {
        private readonly IShowReadService _showReadService;
        private readonly IApiSettingsProvider _settingsProvider;

        public ShowController(IShowReadService showReadService, IApiSettingsProvider settingsProvider)
        {
            _showReadService = showReadService;
            _settingsProvider = settingsProvider;
        }

        /// <summary>
        /// Show data from TvMaze scraper
        /// </summary>
        /// <param name="query">Show name query</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Show>>> Get([FromQuery]string query, [FromQuery]int? page, [FromQuery]int? pageSize)
        {
            if (page.GetValueOrDefault() < 0 || pageSize.GetValueOrDefault() < 0)
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

            var result = (await _showReadService.GetShowAsync(
                !string.IsNullOrWhiteSpace(query) ? (show => show.Name == query) : (Expression<Func<Show, bool>>)null,
                page.Value,
                pageSize.Value
                ))?.ToList();

            if (result == null || !result.Any())
            {
                return NotFound();
            }

            result.ForEach(Ordering);

            return Ok(result);
        }


        /// <summary>
        /// Show data from TvMaze scraper by id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Show>> Get([Required]int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var result = await _showReadService.GetShowAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            Ordering(result);
            return Ok(result);
        }

        private void Ordering(Show show)
        {
            show.Cast = show.Cast?.OrderBy(c => c.Birthday);
        }
    }
}
