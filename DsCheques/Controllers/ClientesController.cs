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
using Microsoft.AspNetCore.Authorization;

namespace DsCheques.Controllers
{
    [Authorize]
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
            //return View(this.clienteRepository.GetClienteAsync(User.Identity.Name));
            return View(this.clienteRepository.GetAll().Where(u =>u.User.UserName == User.Identity.Name));
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ClienteNotFound");
            }

            var cliente = await this.clienteRepository.GetByIdAsync(id.Value);
            if (cliente == null)
            {
                return new NotFoundViewResult("ClienteNotFound");
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
                cliente.User = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);

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
                return new NotFoundViewResult("ClienteNotFound");
            }

            var cliente = await this.clienteRepository.GetByIdAsync(id.Value);
            if (cliente == null)
            {
                return new NotFoundViewResult("ClienteNotFound");
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
                return new NotFoundViewResult("ClienteNotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    cliente.User = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await this.clienteRepository.UpdateAsync(cliente);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.clienteRepository.ExistAsync(cliente.Id))
                    {
                        return new NotFoundViewResult("ClienteNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return new NotFoundViewResult("ClienteNotFound");
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ClienteNotFound");
            }

            var cliente = await this.clienteRepository.GetByIdAsync(id.Value);
            if (cliente == null)
            {
                return new NotFoundViewResult("ClienteNotFound");
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

        public IActionResult ClienteNotFound()
        {
            return this.View();
        }

    }
}
