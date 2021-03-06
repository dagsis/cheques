﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DsCheques.Data;
using DsCheques.Data.Entities;
using DsCheques.Data.Repositories.Interfaces;
using DsCheques.Helpers;
using DsCheques.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace DsCheques.Controllers
{
    [Authorize]
    public class ChequesController : Controller
    {
        CultureInfo cultures = new CultureInfo("es-MX");

        private readonly IChequeRepository chequesRepository;
        private readonly IUserHelper userHelper;

        public ChequesController(IChequeRepository chequesRepository, IUserHelper userHelper)
        {
            this.chequesRepository = chequesRepository;
            this.userHelper = userHelper;
        }

        // GET: Cheques
        public IActionResult Index()
        {
            return View(this.chequesRepository.GetAll().Where(u => u.User.UserName == User.Identity.Name).Include(c => c.Cliente).OrderByDescending(f => f.Id));
        }

        // GET: Cheques/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cheque =  this.chequesRepository.GetChequesWihClientes(id.Value);
            if (cheque == null)
            {
                return new NotFoundViewResult("ChequeNotFound");
            }

            return View(cheque);
        }

        // GET: Cheques/Create
        public IActionResult Create()
        {
            var model = new ChequesViewModel
            {
                FechaIngreso = DateTime.Today,
                FechaDeposito = DateTime.Today,
                Destino = "En cartera",
                Clientes = this.chequesRepository.GetComboClientes(User.Identity.Name)
            };

            return View(model);
        }

        // POST: Cheques/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChequesViewModel view)
        {

            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (view.ImageFile != null && view.ImageFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Cheques",
                        file);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await view.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/cheques/{file}";

                }

               view.User = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);
               decimal importe = Convert.ToDecimal(view.Importe,cultures);
               view.Importe = importe;          

                var cheque = this.toCheques(view, path);
                await this.chequesRepository.CreateAsync(cheque);
                return RedirectToAction(nameof(Index));
           
            }
            return View(view);
        }

        private Cheque toCheques(ChequesViewModel view,string path)
        {
            return new Cheque
            {
                Id = view.Id,
                ImageUrl = path,
                Destino = view.Destino,
                FechaIngreso = view.FechaIngreso,
                FechaDeposito = view.FechaDeposito,
                Firmante = view.Firmante,
                ClienteId = view.ClienteId,
                Cliente = view.Cliente,
                Importe = view.Importe,
                Numero = view.Numero,
                User = view.User
            };
        }

        // GET: Cheques/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ChequeNotFound");
            }

            var cheque = await this.chequesRepository.GetByIdAsync(id.Value);
            if (cheque == null)
            {
                return new NotFoundViewResult("ChequeNotFound");
            }

            var view = this.ToChequeViewModel(cheque);
            return View(view);
        }

        private ChequesViewModel ToChequeViewModel(Cheque cheque)
        {
            return new ChequesViewModel
            {
                Id = cheque.Id,
                ImageUrl = cheque.ImageUrl,
                Destino = cheque.Destino,
                FechaIngreso = cheque.FechaIngreso,
                FechaDeposito = cheque.FechaDeposito,
                Firmante = cheque.Firmante,  
                Clientes =  this.chequesRepository.GetComboClientes(User.Identity.Name),
                ClienteId = cheque.ClienteId,
                Importe = cheque.Importe,
                Numero = cheque.Numero,
                User = cheque.User
            };
        }

        // POST: Cheques/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ChequesViewModel view)
        {       
            if (ModelState.IsValid)
            {
                try
                {
                    var path = view.ImageUrl;

                    if (view.ImageFile != null && view.ImageFile.Length > 0)
                    {
                        var guid = Guid.NewGuid().ToString();
                        var file = $"{guid}.jpg";

                        path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\Cheques",
                            file);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await view.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/Cheques/{file}";
                    }

                    view.User = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    decimal importe = Convert.ToDecimal(view.Importe, cultures);
                    view.Importe = importe;

                    var cheque = this.toCheques(view, path);
                    await this.chequesRepository.UpdateAsync(cheque);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.chequesRepository.ExistAsync(view.Id))
                    {
                        return new NotFoundViewResult("ChequeNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(view);
        }

        // GET: Cheques/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ChequeNotFound");
            }

            var cheque = await this.chequesRepository.GetByIdAsync(id.Value);

            if (cheque == null)
            {
                return new NotFoundViewResult("ChequeNotFound");
            }

            return View(cheque);
        }

        // POST: Cheques/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cheque = await this.chequesRepository.GetByIdAsync(id);
            await this.chequesRepository.DeleteAsync(cheque);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ChequeNotFound()
        {
            return this.View();
        }

    }
}
