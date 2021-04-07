using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Accepted.Services;
using Accepted.Models;
using AutoMapper;
using Accepted.DTOs;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Accepted.FluentValidation;

namespace Accepted.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchesService _matchService;
        private readonly IMapper _mapper;

        public static IActionResult ValidationError(ValidationErrorsException ex) => new JsonResult(ex.Errors) { StatusCode = 422 };

        public MatchesController(IMatchesService matchService, IMapper mapper)
        {
            _matchService = matchService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all matches
        /// </summary>
        /// <response code="200">List of Items</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<MatchDto>))]
        public async Task<ActionResult<IEnumerable<MatchDto>>> GetAll()
        {
            var list =  await _matchService.Get();
            return list.Select( m => _mapper.Map<MatchDto>(m) ).ToList();
        }

        /// <summary>
        /// Get match by id
        /// </summary>
        /// <param name="id">Match id</param>
        /// <response code="200">Item</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MatchDto))]
        public async Task<ActionResult<MatchDto>> Get(int id)
        {
            var match = await _matchService.Get(id);
            var matchDto = _mapper.Map<MatchDto>(match);

            if (match == null)
            {
                return NotFound();
            }

            return matchDto;
        }

        /// <summary>
        /// Change match
        /// </summary>
        /// <response code="204">Item changed</response>
        /// <response code="400">Change item failed</response>  
        /// <response code="422">Item is not valid</response>  
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationErrorList))]
        public async Task<IActionResult> Put(int id, MatchDto match)
        {
            try
            {
                await _matchService.Save(id, _mapper.Map<Match>(match));
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
        /// Add new match
        /// </summary>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">Add item failed</response>  
        /// <response code="422">Item is not valid</response>  
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(MatchDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationErrorList))]
        public async Task<IActionResult> Post(MatchDto matchDto)
        {
            try
            {
                var match = await _matchService.Add(_mapper.Map<MatchDto, Match>(matchDto));
                matchDto = _mapper.Map<MatchDto>(match);
                return CreatedAtAction("Get", new { id = matchDto.Id }, matchDto);
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
        /// <param name="id">Match id</param>
        /// <response code="204">Item deleted</response>
        /// <response code="400">Delete item failed</response>  
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MatchDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MatchDto>> Delete(int id)
        {
            try
            {
                return _mapper.Map<MatchDto>(await _matchService.Delete(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
