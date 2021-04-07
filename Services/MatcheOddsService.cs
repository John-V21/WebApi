using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Accepted.DBContext;
using Accepted.Models;
using FluentValidation;
using Accepted.FluentValidation;

namespace Accepted.Services
{
    public class MatchOddsService : IMatchOddsService
    {
        private readonly AppDbContext _context;
        private readonly FluentValidator _modelValidators;

        public MatchOddsService(AppDbContext context, FluentValidator modelValidators)
        {
            _context = context;
            _modelValidators = modelValidators;
        }

        public async Task<IEnumerable<MatchOdd>> Get()
        {
            return await _context.MatchOdds.ToListAsync();
        }

        public async Task<MatchOdd> Get(int id)
        {
            return await _context.MatchOdds.FindAsync(id);
        }

        public async Task Save(int id, MatchOdd matchOdd)
        {
            if (id != matchOdd.Id)
            {
                throw new ArgumentException("Invalid id");
            }

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

        public async Task<MatchOdd> Add(MatchOdd matchOdd)
        {
            try
            {
                _modelValidators.ThrowIfInvalid(matchOdd);

                var addedMatchOdd = _context.MatchOdds.Add(matchOdd);
                await _context.SaveChangesAsync();
                return addedMatchOdd.Entity;
            }
            catch (DbUpdateException ex)
            {
                throw new ApplicationException(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<MatchOdd> Delete(int id)
        {
            var matchOdd = await _context.MatchOdds.FindAsync(id);
            if (matchOdd == null)
            {
                throw new ApplicationException("Key does not exists");
            }

            _context.MatchOdds.Remove(matchOdd);
            await _context.SaveChangesAsync();

            return matchOdd;
        }

        private bool Exists(int id)
        {
            return _context.MatchOdds.Any(e => e.Id == id);
        }

        public IEnumerable<MatchOdd> GetByMatch(int id)
        {
            return _context.MatchOdds.Where(mo => mo.MatchId == id).ToList();
        }
    }
}
