using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DsCheques.Data;
using DsCheques.Data.Entities;
using DsCheques.Helpers;
using DsCheques.Data.Repositories.Interfaces;

namespace DsCheques.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IClienteRepository clienteRepository;
        private readonly IUserHelper userHelper;

        public ClientesController(IClienteRepository clienteRepository, IUserHelper userHelper)
        {
            this.clienteRepository = clienteRepository;
            this.userHelper = userHelper;
        }

        // GET: Clientes
        public IActionResult Index()
        {
            return View(this.clienteRepository.GetAll());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await this.clienteRepository.GetByIdAsync(id.Value);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            //TODO: agregar usuario al cliente y tambien en el edit
            if (ModelState.IsValid)
            {
                cliente.User = await this.userHelper.GetUserByEmailAsync("dagsis@dagsis.com.ar");
                await this.clienteRepository.CreateAsync(cliente);
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await this.clienteRepository.GetByIdAsync(id.Value);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Cuit")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    cliente.User = await this.userHelper.GetUserByEmailAsync("dagsis@dagsis.com.ar");
                    await this.clienteRepository.UpdateAsync(cliente);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.clienteRepository.ExistAsync(cliente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await this.clienteRepository.GetByIdAsync(id.Value);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             var cliente = await this.clienteRepository.GetByIdAsync(id);
            await this.clienteRepository.DeleteAsync(cliente);
            return RedirectToAction(nameof(Index));
        }

    }
}
