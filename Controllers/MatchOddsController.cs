using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accepted.DBContext;
using Accepted.Models;
using Accepted.DTOs;
using AutoMapper;
using Accepted.Services;
using System.Net.Mime;

namespace Accepted.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchOddsController : ControllerBase
    {
        private readonly IMatchOddsService _matchOddsService;
        private readonly IMapper _mapper;

        public MatchOddsController(IMatchOddsService matchService, IMapper mapper)
        {
            _matchOddsService = matchService;
            _mapper = mapper;
        }

        // GET: api/Matches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchOddDto>>> GetMAll()
        {
            var list = await _matchOddsService.Get();
            return list.Select(m => _mapper.Map<MatchOddDto>(m)).ToList();
        }

        // GET: api/Matches/5
        [HttpGet("{id}")]
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

        [HttpGet("match/{id}")]
        public ActionResult<IEnumerable<MatchOddDto>> GetByMatch(int id)
        {
            var matchOdds = _matchOddsService.GetByMatch(id);
            return matchOdds.Select(mo => _mapper.Map<MatchOddDto>(mo)).ToList();
        }


        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, MatchOddDto match)
        {
            try
            {
                await _matchOddsService.Save(id, _mapper.Map<MatchOdd>(match));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(MatchOddDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Match>> Post(MatchOddDto MatchOddDto)
        {
            try
            {
                var match = await _matchOddsService.Add(_mapper.Map<MatchOddDto, MatchOdd>(MatchOddDto));
                MatchOddDto = _mapper.Map<MatchOddDto>(match);
                return CreatedAtAction("Get", new { id = MatchOddDto.Id }, MatchOddDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Matches/5
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
