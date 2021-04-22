using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Accepted.DTOs;
using Accepted.Services;
using System.Net.Mime;
using Accepted.FluentValidation;

namespace Accepted.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class MatchOddsController : ControllerBase
    {
        private readonly IMatchOddsService _matchOddsService;
        public static IActionResult ValidationError(FluentValidationException  ex) => new JsonResult(ex.Errors) { StatusCode = 422 };

        public MatchOddsController(IMatchOddsService matchService)
        {
            _matchOddsService = matchService;
        }

        /// <summary>
        /// Get all matches
        /// </summary>
        /// <response code="200">List of Items</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<MatchOddDto>))]
        public async Task<ActionResult<IEnumerable<MatchOddDto>>> GetMAll()
        {
            return new JsonResult(await _matchOddsService.Get());
        }

        /// <summary>
        /// Get match odd by id
        /// </summary>
        /// <param name="id">Match Odd id</param>
        /// <response code="200">Item</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MatchOddDto))]
        public async Task<ActionResult<MatchOddDto>> Get(int id)
        {
            var match = await _matchOddsService.Get(id);
            if (match == null)
            {
                return NotFound();
            }

            return match;
        }

        /// <summary>
        /// Get a list of match odds by match id
        /// </summary>
        /// <param name="id">Match id</param>
        /// <response code="200">Item</response>
        [HttpGet("match/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<MatchOddDto>))]
        public ActionResult<IEnumerable<MatchOddDto>> GetByMatch(int id)
        {
            return _matchOddsService.GetByMatch(id).ToList();
        }

        /// <summary>
        /// Change match Odd
        /// </summary>
        /// <response code="204">Item changed</response>
        /// <response code="400">Change item failed</response>  
        /// <response code="422">Item is not valid</response>  
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(FluentValidationErrorList))]
        public async Task<IActionResult> Put(int id, MatchOddDto match)
        {
            try
            {
                await _matchOddsService.Save(id, match);
            }
            catch (FluentValidationException  ve)
            {
                return ValidationError(ve);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        /// <summary>
        /// Add new match odd
        /// </summary>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">Add item failed</response>  
        /// <response code="422">Item is not valid</response>  
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(MatchOddDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(FluentValidationErrorList))]
        public async Task<IActionResult> Post(MatchOddDto matchOddDto)
        {
            try
            {
                var match = await _matchOddsService.Add(matchOddDto);
                return CreatedAtAction("Get", new { id = match.Id }, match);
            }
            catch (FluentValidationException  ve)
            {
                return ValidationError(ve);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete match by id
        /// </summary>
        /// <param name="id">Match Odd id</param>\
        /// <response code="204">Item deleted</response>
        /// <response code="400">Delete item failed</response>  
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MatchOddDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MatchOddDto>> Delete(int id)
        {
            try
            {
                return  await _matchOddsService.Delete(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
