using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accepted.DBContext;
using Accepted.Models;
using Accepted.FluentValidation;
using AutoMapper;
using Accepted.DTOs;

namespace Accepted.Services
{
    public class MatchesService : IMatchesService
    {
        private readonly AppDbContext _context;
        private readonly FluentValidator _modelValidators;
        private readonly IMapper _mapper;

        public MatchesService(AppDbContext context, FluentValidator modelValidators, IMapper mapper)
        {
            _context = context;
            _modelValidators = modelValidators;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MatchDto>> Get()
        {
            return await _context.Matches.Select(m => _mapper.Map<MatchDto>(m)).ToListAsync();
        }

        public async Task<MatchDto> Get(int id)
        {
            return _mapper.Map<MatchDto>(await _context.Matches.FindAsync(id));
        }

        public async Task Save(int id, MatchDto matchDto)
        {
            if (id != matchDto.Id)
            {
                throw new ApplicationException("Invalid Id");
            }
            var match = _mapper.Map<Match>(matchDto);

            _modelValidators.ThrowIfInvalid(match);

            _context.Entry(match).State = EntityState.Modified;

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

        public async Task<MatchDto> Add(MatchDto matchDto)
        {
            try
            {
                var match = _mapper.Map<Match>(matchDto);

                _modelValidators.ThrowIfInvalid(match);

                var addedMatch = _context.Matches.Add(match);
                await _context.SaveChangesAsync();
                return _mapper.Map<MatchDto>(addedMatch.Entity);
            }
            catch(DbUpdateException ex)
            {
                throw new ApplicationException(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<MatchDto> Delete(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                throw new ApplicationException("Key does not exists");
            }

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();

            return _mapper.Map<MatchDto>(match);
        }

        private bool Exists(int id)
        {
            return _context.Matches.Any(e => e.Id == id);
        }
    }
}
