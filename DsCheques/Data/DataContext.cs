using DsCheques.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DsCheques.Data
{
    public class DataContext : IdentityDbContext<User>

    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Cheque> Cheques { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
    }
}
