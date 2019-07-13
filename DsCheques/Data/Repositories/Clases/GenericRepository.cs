using DsCheques.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DsCheques.Data.Repositories.Clases
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        public Task<T> CreateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
