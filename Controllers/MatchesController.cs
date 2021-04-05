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

namespace Accepted.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchesService _matchService;
        private readonly IMapper _mapper;

        public MatchesController(IMatchesService matchService, IMapper mapper)
        {
            _matchService = matchService;
            _mapper = mapper;
        }

        // GET: api/Matches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchDto>>> GetAll()
        {
            var list =  await _matchService.Get();
            return list.Select( m => _mapper.Map<MatchDto>(m) ).ToList();
        }

        // GET: api/Matches/5
        [HttpGet("{id}")]
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

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, MatchDto match)
        {
            try
            {
                await _matchService.Save(id, _mapper.Map<Match>(match));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(MatchDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Match>> Post(MatchDto matchDto)
        {
            try
            {
                var match = await _matchService.Add(_mapper.Map<MatchDto, Match>(matchDto));
                matchDto = _mapper.Map<MatchDto>(match);
                return CreatedAtAction("Get", new { id = matchDto.Id }, matchDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Matches/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MatchDto), StatusCodes.Status201Created)]
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
