using DsCheques.Data.Entities;
using DsCheques.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DsCheques.Data.Repositories.Clases
{
    public class ClienteRepository : GenericRepository<Cliente>,IClienteRepository
    {
        public ClienteRepository(DataContext context) : base(context)
        {
        }
    }
}
