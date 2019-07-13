using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DsCheques.Data;
using DsCheques.Data.Entities;

namespace DsCheques.Controllers
{
    public class CompaniasController : Controller
    {
        private readonly DataContext _context;

        public CompaniasController(DataContext context)
        {
            _context = context;
        }

        // GET: Companias
        public async Task<IActionResult> Index()
        {
            return View(await _context.Companias.ToListAsync());
        }

        // GET: Companias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compania = await _context.Companias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compania == null)
            {
                return NotFound();
            }

            return View(compania);
        }

        // GET: Companias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Compania compania)
        {
            if (ModelState.IsValid)
            {
                _context.Add(compania);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(compania);
        }

        // GET: Companias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compania = await _context.Companias.FindAsync(id);
            if (compania == null)
            {
                return NotFound();
            }
            return View(compania);
        }

        // POST: Companias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Compania compania)
        {
            if (id != compania.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compania);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompaniaExists(compania.Id))
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
            return View(compania);
        }

        // GET: Companias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compania = await _context.Companias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compania == null)
            {
                return NotFound();
            }

            return View(compania);
        }

        // POST: Companias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compania = await _context.Companias.FindAsync(id);
            _context.Companias.Remove(compania);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompaniaExists(int id)
        {
            return _context.Companias.Any(e => e.Id == id);
        }
    }
}
