using DsCheques.Data.Entities;
using DsCheques.Data.Repositories.Interfaces;
using DsCheques.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DsCheques.Data.Repositories.Clases
{
    public class ClienteRepository : GenericRepository<Cliente>,IClienteRepository
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;

        public ClienteRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            this.context = context;
            this.userHelper = userHelper;
        }

        public async Task<IQueryable<Cliente>> GetClienteAsync(string userName)
        {
            var user = await this.userHelper.GetUserByEmailAsync(userName);

            return this.context.Clientes
                .Where(o => o.User == user)
                .OrderByDescending(o => o.Name);
        }
    }
}
