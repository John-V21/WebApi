using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Accepted.DBContext;
using Accepted.Models;
using FluentValidation;
using Accepted.FluentValidation;
using AutoMapper;
using Accepted.DTOs;

namespace Accepted.Services
{
    public class MatchOddsService : IMatchOddsService
    {
        private readonly AppDbContext _context;
        private readonly FluentValidator _modelValidators;
        private readonly IMapper _mapper;

        public MatchOddsService(AppDbContext context, FluentValidator modelValidators, IMapper mapper)
        {
            _context = context;
            _modelValidators = modelValidators;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MatchOddDto>> Get()
        {
            return await _context.MatchOdds.Select(m => _mapper.Map<MatchOddDto>(m)).ToListAsync();
        }

        public async Task<MatchOddDto> Get(int id)
        {
            return _mapper.Map<MatchOddDto>(await _context.MatchOdds.FindAsync(id));
        }

        public async Task Save(int id, MatchOddDto matchOddDto)
        {
            if (id != matchOddDto.Id)
            {
                throw new ArgumentException("Invalid id");
            }

            var matchOdd = _mapper.Map<MatchOdd>(matchOddDto);

            _modelValidators.ThrowIfInvalid(matchOdd);

            _context.Entry(matchOdd).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!Exists(id))
                {
                    throw new ApplicationException("Key does not exists");
                }
                else
                {
                    throw new ApplicationException(ex.Message);
                }
            }
        }

        public async Task<MatchOddDto> Add(MatchOddDto matchOddDto)
        {
            try
            {
                var matchOdd = _mapper.Map<MatchOdd>(matchOddDto);
                _modelValidators.ThrowIfInvalid(matchOdd);

                var addedMatchOdd = _context.MatchOdds.Add(matchOdd);
                await _context.SaveChangesAsync();
                return _mapper.Map<MatchOddDto>(addedMatchOdd.Entity);
            }
            catch (DbUpdateException ex)
            {
                throw new ApplicationException(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<MatchOddDto> Delete(int id)
        {
            var matchOdd = await _context.MatchOdds.FindAsync(id);
            if (matchOdd == null)
            {
                throw new ApplicationException("Key does not exists");
            }

            _context.MatchOdds.Remove(matchOdd);
            await _context.SaveChangesAsync();

            return _mapper.Map<MatchOddDto>(matchOdd);
        }

        private bool Exists(int id)
        {
            return _context.MatchOdds.Any(e => e.Id == id);
        }

        public IEnumerable<MatchOddDto> GetByMatch(int id)
        {
            return _context.MatchOdds.Where(mo => mo.MatchId == id).Select(m => _mapper.Map<MatchOddDto>(m)).ToList();
        }
    }
}
