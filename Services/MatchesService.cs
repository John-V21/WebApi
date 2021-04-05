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
    public class MatchesService : IMatchesService
    {
        private readonly AppDbContext _context;

        public MatchesService(AppDbContext context)
        {
            _context = context;
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

        public async Task<Match> Add(Match match)
        {
            var addedMatch = _context.Matches.Add(match);
            await _context.SaveChangesAsync();
            return addedMatch.Entity;
        }

        public async Task<Match> Delete(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                throw new KeyNotFoundException();
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
