using DsCheques.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DsCheques.Data.Repositories.Interfaces
{
  public interface IClienteRepository : IGenericRepository<Cliente>
    {
        Task<IQueryable<Cliente>> GetClienteAsync(string userName);

    }
}
