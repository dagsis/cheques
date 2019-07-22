using DsCheques.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DsCheques.Common.Services
{
 public class PickerService
    {
        public static List<Cliente> GetClientes()
        {
            var clientes = new List<Cliente>()
            {
                new Cliente() { Id = 1,Name = "Nico"},
                new Cliente() { Id = 2,Name = "Fernando Gestion"},
                new Cliente() { Id = 3,Name = "Federico"},
            };

            return clientes;
        }
    }
}
