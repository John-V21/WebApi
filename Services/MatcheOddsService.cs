using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accepted.DBContext;
using Accepted.Models;

namespace Accepted.Services
{
    public class MatchOddsService : IMatchOddsService
    {
        private readonly AppDbContext _context;

        public MatchOddsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MatchOdd>> Get()
        {
            return await _context.MatchOdds.ToListAsync();
        }

        public async Task<MatchOdd> Get(int id)
        {
            return await _context.MatchOdds.FindAsync(id);
        }

        public async Task Save(int id, MatchOdd match)
        {
            if (id != match.Id)
            {
                throw new ArgumentException("Invalid id");
            }

            _context.Entry(match).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(id))
                {
                    throw new KeyNotFoundException();
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<MatchOdd> Add(MatchOdd matchOdd)
        {
            var addedMatchOdd = _context.MatchOdds.Add(matchOdd);
            await _context.SaveChangesAsync();
            return addedMatchOdd.Entity;
        }

        public async Task<MatchOdd> Delete(int id)
        {
            var matchOdd = await _context.MatchOdds.FindAsync(id);
            if (matchOdd == null)
            {
                throw new KeyNotFoundException();
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
