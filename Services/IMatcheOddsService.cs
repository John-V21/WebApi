using Accepted.DTOs;
using Accepted.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accepted.Services
{
    public interface IMatchOddsService : ICRUDService<MatchOddDto, int>
    {
        IEnumerable<MatchOddDto> GetByMatch(int id);
    }
}