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
    public class VisitorsController : Controller
    {
        private readonly AquaparkDbContext _context;

        public VisitorsController(AquaparkDbContext context)
        {
            _context = context;
        }

        // GET: Visitors
        public async Task<IActionResult> Index()
        {
              return _context.Visitors != null ? 
                          View(await _context.Visitors.ToListAsync()) :
                          Problem("Entity set 'AquaparkDbContext.Visitors'  is null.");
        }

        // GET: Visitors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Visitors == null)
            {
                return NotFound();
            }

            var visitor = await _context.Visitors
                .FirstOrDefaultAsync(m => m.VisitorId == id);
            if (visitor == null)
            {
                return NotFound();
            }

            return View(visitor);
        }

        

        // GET: Visitors/Create
        public IActionResult Create()
        {
            List<Visitor> list = _context.Visitors.ToList();
            int c = list.Count();
            ViewBag.VisitorId = list.ElementAt(c - 1).VisitorId + 1;
            return View();
        }

        // POST: Visitors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,VisitorId,BirthDate,Height,Weight")] Visitor visitor)
        {
            visitor.Status = 1;
            if (ModelState.IsValid)
            {
                _context.Add(visitor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(visitor);
        }

        // GET: Visitors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Visitors == null)
            {
                return NotFound();
            }

            var visitor = await _context.Visitors.FindAsync(id);
            ViewBag.ErrorString = "";
            if (visitor == null)
            {
                return NotFound();
            }
            return View(visitor);
        }

        // POST: Visitors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,VisitorId,BirthDate,Height,Weight,Status")] Visitor visitor)
        {
            if (id != visitor.VisitorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var tickets = _context.Tickets.Where(t => t.TicketStatus == 1).ToList();
                    if (tickets.Any())
                    {
                        ViewBag.ErrorString += "Заборонено редагувати дані відвідувача під час його перебування в аквапарку. ";
                        return View(visitor);
                    }
                    _context.Update(visitor);
                    if (visitor.Status == 0)
                    {
                        var relatedTickets = await _context.Tickets.Where(t => t.TicketOwner == id & t.TicketStatus == 1).ToListAsync();
                        foreach (var t in relatedTickets)
                        {
                            t.TicketStatus = 0;
                            _context.Update(t);
                        }
                    }
                 
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisitorExists(visitor.VisitorId))
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
            return View(visitor);
        }

        // GET: Visitors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Visitors == null)
            {
                return NotFound();
            }

            var visitor = await _context.Visitors
                .FirstOrDefaultAsync(m => m.VisitorId == id);
            if (visitor == null)
            {
                return NotFound();
            }

            return View(visitor);
        }

        // POST: Visitors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Visitors == null)
            {
                return Problem("Entity set 'AquaparkDbContext.Visitors'  is null.");
            }
            var visitor = await _context.Visitors.FindAsync(id);
            if (visitor != null)
            {
                var relatedTickets = await _context.Tickets.Where(t => t.TicketOwner == id).ToListAsync();
                int tId;
                Ticket ticket;
                while (relatedTickets.Any())
                {
                    tId = relatedTickets.FirstOrDefault().TicketId;
                    ticket = _context.Tickets.FirstOrDefault(t => t.TicketId == tId);
                    _context.Tickets.Remove(ticket);
                    relatedTickets.Remove(ticket);
                }
                _context.Visitors.Remove(visitor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisitorExists(int id)
        {
          return (_context.Visitors?.Any(e => e.VisitorId == id)).GetValueOrDefault();
        }
    }
}
