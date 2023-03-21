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
    public class TicketsController : Controller
    {
        private readonly AquaparkDbContext _context;

        public TicketsController(AquaparkDbContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var aquaparkDbContext = _context.Tickets.Include(t => t.Location).Include(t => t.LocationNavigation).Include(t => t.TicketOwnerNavigation);
            return View(await aquaparkDbContext.ToListAsync());
        }

        

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Location)
                .Include(t => t.LocationNavigation)
                .Include(t => t.TicketOwnerNavigation)
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            //SelectList locations = new SelectList(_context.Halls, "HallId", "HallId");
            //locations.Concat(new SelectList(_context.Slides, "SlideId", "SlideId"));
            List<string> types = new List<string>{ "slide", "hall" };
            ViewData["LocationType"] = new SelectList(types);
            //ViewData["LocationId"] = new SelectList(_context.Slides, "SlideId", "SlideId");
            //ViewData["LocationId"] = locations;
            ViewData["TicketOwner"] = new SelectList(_context.Visitors, "VisitorId", "VisitorId");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string type, byte id, [Bind("TicketId,LocationHall,LocationSlide,TicketOwner,Price")] Ticket ticket)
        {
            ticket.TicketStatus = 1;
            if (type == "slide")
            {
                ticket.LocationHall = null;
                ticket.LocationSlide = id;
            }
            else if (type == "slide")
            {
                ticket.LocationHall = id;
                ticket.LocationSlide = null;
            }
            //if (ticket.LocationHall == 1) ViewData["LocationId"] = new SelectList(_context.Halls, "HallId", "HallId", ticket.LocationId);
            //else if (ticket.LocationSlide == 1) ViewData["LocationId"] = new SelectList(_context.Slides, "SlideId", "SlideId", ticket.LocationId);
            //ViewData["TicketOwner"] = new SelectList(_context.Visitors, "VisitorId", "VisitorId", ticket.TicketOwner);

            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["LocationHall"] = new SelectList(_context.Halls, "HallId", "HallId", ticket.LocationHall);
            ViewData["LocationSlide"] = new SelectList(_context.Slides, "SlideId", "SlideId", ticket.LocationSlide);
            ViewData["TicketOwner"] = new SelectList(_context.Visitors, "VisitorId", "VisitorId", ticket.TicketOwner);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketId,LocationHall,LocationSlide,TicketOwner,TicketStatus,Price")] Ticket ticket)
        {
            //LocationType must be managed!!
            if (id != ticket.TicketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.TicketId))
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
            ViewData["LocationHall"] = new SelectList(_context.Halls, "HallId", "HallId", ticket.LocationHall);
            ViewData["LocationSlide"] = new SelectList(_context.Slides, "SlideId", "SlideId", ticket.LocationSlide);
            ViewData["TicketOwner"] = new SelectList(_context.Visitors, "VisitorId", "VisitorId", ticket.TicketOwner);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Location)
                .Include(t => t.LocationNavigation)
                .Include(t => t.TicketOwnerNavigation)
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'AquaparkDbContext.Tickets'  is null.");
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
          return (_context.Tickets?.Any(e => e.TicketId == id)).GetValueOrDefault();
        }
    }
}
