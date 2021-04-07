using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accepted.DBContext;
using Accepted.Models;
using Accepted.FluentValidation;

namespace Accepted.Services
{
    public class MatchesService : IMatchesService
    {
        private readonly AppDbContext _context;
        private readonly FluentValidator _modelValidators;


        public MatchesService(AppDbContext context, FluentValidator modelValidators)
        {
            _context = context;
            _modelValidators = modelValidators;

        }

        public async Task<IEnumerable<Match>> Get()
        {
            return await _context.Matches.ToListAsync();
        }

        public async Task<Match> Get(int id)
        {
            return await _context.Matches.FindAsync(id);
        }

        public async Task Save(int id, Match match)
        {
            if (id != match.Id)
            {
                throw new ApplicationException("Invalid Id");
            }

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

        public async Task<Match> Add(Match match)
        {
            try
            {
                _modelValidators.ThrowIfInvalid(match);

                var addedMatch = _context.Matches.Add(match);
                await _context.SaveChangesAsync();
                return addedMatch.Entity;
            }
            catch(DbUpdateException ex)
            {
                throw new ApplicationException(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<Match> Delete(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                throw new ApplicationException("Key does not exists");
            }

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();

            return match;
        }

        private bool Exists(int id)
        {
            return _context.Matches.Any(e => e.Id == id);
        }
    }
}
