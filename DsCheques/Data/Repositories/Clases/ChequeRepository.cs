using DsCheques.Data.Entities;
using DsCheques.Data.Repositories.Interfaces;
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
        public ChequeRepository(DataContext context) : base(context)
        {
            this.context = context;
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
