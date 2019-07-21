using DsCheques.Data.Entities;
using DsCheques.Data.Repositories.Interfaces;
using DsCheques.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DsCheques.Data.Repositories.Clases
{
    public class ChequeRepository : GenericRepository<Cheque>, IChequeRepository
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;

        public ChequeRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            this.context = context;
            this.userHelper = userHelper;
        }

        public IQueryable GetAllChequesByOrder()
        {
            return this.context.Cheques.Include(c => c.Cliente).Include(u => u.User).AsNoTracking();
        }

        public async Task<IQueryable<Cheque>> GetChequeAsync(string userName)
        {
            var user = await this.userHelper.GetUserByEmailAsync(userName);
           
            return this.context.Cheques
                .Where(o => o.User == user)
                .OrderByDescending(o => o.FechaDeposito);
        }

        public Cheque GetChequesWihClientes(int id)
        {
            return  this.context.Cheques
                .Include(c=>c.Cliente)               
                .FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<SelectListItem> GetComboClientes()
        {
            var list = this.context.Clientes.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Seleccione un Cliente...)",
                Value = "0"
            });

            return list;
        }
    }
}

