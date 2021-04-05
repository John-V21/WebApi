using Accepted.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accepted.Services
{
    public interface ICRUDService<T, K>
    {
        Task<T> Add(T model);
        Task<T> Delete(K id);
        Task<IEnumerable<T>> Get();
        Task<T> Get(K id);
        Task Save(K id, T match);
    }
}