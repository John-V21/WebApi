using Accepted.DTOs;
using Accepted.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accepted.Services
{
    public interface IMatchOddsService : ICRUDService<MatchOdd, int>
    {
        IEnumerable<MatchOdd> GetByMatch(int id);
    }
}