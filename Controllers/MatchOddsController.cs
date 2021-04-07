using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Accepted.Models;
using Accepted.DTOs;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public static IActionResult ValidationError(ValidationErrorsException ex) => new JsonResult(ex.Errors) { StatusCode = 422 };

        public MatchOddsController(IMatchOddsService matchService, IMapper mapper)
        {
            _matchOddsService = matchService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all matches
        /// </summary>
        /// <response code="200">List of Items</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<MatchOddDto>))]
        public async Task<ActionResult<IEnumerable<MatchOddDto>>> GetMAll()
        {
            var list = await _matchOddsService.Get();
            return list.Select(m => _mapper.Map<MatchOddDto>(m)).ToList();
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
            var MatchOddDto = _mapper.Map<MatchOddDto>(match);

            if (match == null)
            {
                return NotFound();
            }

            return MatchOddDto;
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
            var matchOdds = _matchOddsService.GetByMatch(id);
            return matchOdds.Select(mo => _mapper.Map<MatchOddDto>(mo)).ToList();
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
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationErrorList))]
        public async Task<IActionResult> Put(int id, MatchOddDto match)
        {
            try
            {
                await _matchOddsService.Save(id, _mapper.Map<MatchOdd>(match));
            }
            catch (ValidationErrorsException ve)
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
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationErrorList))]
        public async Task<IActionResult> Post(MatchOddDto MatchOddDto)
        {
            try
            {
                var match = await _matchOddsService.Add(_mapper.Map<MatchOddDto, MatchOdd>(MatchOddDto));
                MatchOddDto = _mapper.Map<MatchOddDto>(match);
                return CreatedAtAction("Get", new { id = MatchOddDto.Id }, MatchOddDto);
            }
            catch (ValidationErrorsException ve)
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
                return  _mapper.Map<MatchOddDto>(await _matchOddsService.Delete(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
