using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TvMazeApi.Proxy.Interfaces;
using TvMazeScraper.Models;

namespace TvMazeScraper.Api.Synchronizer.Controllers
{
    [Route("api/[controller]")]
    public class ProxyShowController : Controller
    {
        private readonly ITvMazeProxy _apiProxy;

        public ProxyShowController(ITvMazeProxy apiProxy)
        {
            _apiProxy = apiProxy;
        }

        /// <summary>
        /// Show data from TvMaze api
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Show>>> Get([FromQuery][Required]string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest();
            }

            var result = (await _apiProxy.GetShowsInfoByNameAsync(query))?.ToList();
            if (result == null || !result.Any())
            {
                return NotFound();
            }
            result.ForEach(Ordering);

            return Ok(result);
        }

        /// <summary>
        /// Show data updates from TvMaze api
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet("Updates")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Show>>> Get([FromQuery][Required]DateTime? date)
        {
            if (!date.HasValue)
            {
                return BadRequest();
            }

            var result = (await _apiProxy.GetShowsUpdatesAsync(date.Value))?.ToList();
            if (result == null || !result.Any())
            {
                return NotFound();
            }
            result.ForEach(Ordering);

            return Ok(result);
        }

        private void Ordering(Show show)
        {
            show.Cast = show.Cast?.OrderBy(c => c.Birthday);
        }
    }
}
