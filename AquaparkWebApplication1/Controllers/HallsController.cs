using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AquaparkWebApplication1.Models;

namespace AquaparkWebApplication1.Controllers
{
    public class HallsController : Controller
    {
        private readonly AquaparkDbContext _context;

        public HallsController(AquaparkDbContext context)
        {
            _context = context;
        }

        // GET: Halls
        public async Task<IActionResult> Index()
        {
              return _context.Halls != null ? 
                          View(await _context.Halls.ToListAsync()) :
                          Problem("Entity set 'AquaparkDbContext.Halls'  is null.");
        }

        // GET: Halls/Details/5
        public async Task<IActionResult> Details(byte? id)
        {
            if (id == null || _context.Halls == null)
            {
                return NotFound();
            }

            var hall = await _context.Halls
                .FirstOrDefaultAsync(m => m.HallId == id);
            if (hall == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index","Pools", new {id=hall.HallId});
        }

        // GET: Halls/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Halls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HallId,PoolsMaxDepth,PoolsMinDepth,HallMaxPeople")] Hall hall)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hall);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hall);
        }

        // GET: Halls/Edit/5
        public async Task<IActionResult> Edit(byte? id)
        {
            if (id == null || _context.Halls == null)
            {
                return NotFound();
            }

            var hall = await _context.Halls.FindAsync(id);
            if (hall == null)
            {
                return NotFound();
            }
            return View(hall);
        }

        // POST: Halls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(byte id, [Bind("HallId,PoolsMaxDepth,PoolsMinDepth,HallMaxPeople")] Hall hall)
        {
            if (id != hall.HallId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hall);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HallExists(hall.HallId))
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
            return View(hall);
        }

        // GET: Halls/Delete/5
        public async Task<IActionResult> Delete(byte? id)
        {
            if (id == null || _context.Halls == null)
            {
                return NotFound();
            }

            var hall = await _context.Halls
                .FirstOrDefaultAsync(m => m.HallId == id);
            if (hall == null)
            {
                return NotFound();
            }

            return View(hall);
        }

        // POST: Halls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(byte id)
        {
            if (_context.Halls == null)
            {
                return Problem("Entity set 'AquaparkDbContext.Halls'  is null.");
            }
            var hall = await _context.Halls.FindAsync(id);
            if (hall != null)
            {
                var relatedPools = await _context.Pools.Where(p => p.Hall == id).ToListAsync();
                var relatedTickets = await _context.Tickets.Where(t => t.LocationId == id).ToListAsync();
                byte pId;
                int tId;
                Pool pool;
                Ticket ticket;
                while (relatedPools.Any())
                {
                    pId = relatedPools.FirstOrDefault().PoolId;
                    pool = _context.Pools.FirstOrDefault(p => p.PoolId == pId);
                    _context.Pools.Remove(pool);
                    relatedPools.Remove(pool);
                }
                while (relatedTickets.Any())
                {
                    tId = relatedTickets.FirstOrDefault().TicketId;
                    ticket = _context.Tickets.FirstOrDefault(t => t.TicketId == tId);
                    _context.Tickets.FirstOrDefault(_ => _.TicketId == tId).TicketStatus = 0;
                    relatedTickets.Remove(ticket);
                }
                _context.Halls.Remove(hall);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HallExists(byte id)
        {
          return (_context.Halls?.Any(e => e.HallId == id)).GetValueOrDefault();
        }
    }
}
