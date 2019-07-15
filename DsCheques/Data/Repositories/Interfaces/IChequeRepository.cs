using DsCheques.Data.Entities;
using DsCheques.Data.Repositories.Clases;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DsCheques.Data.Repositories.Interfaces
{
   public interface IChequeRepository : IGenericRepository<Cheque>
    {
        IEnumerable<SelectListItem> GetComboClientes();

        Cheque GetChequesWihClientes(int id);

        IQueryable GetAllChequesByOrder();
    }
}
