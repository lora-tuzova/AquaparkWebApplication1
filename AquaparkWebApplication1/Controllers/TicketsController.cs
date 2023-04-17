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
            List<Ticket> list = _context.Tickets.ToList();
            int c = list.Count();
            ViewBag.TicketId = list.ElementAt(c - 1).TicketId + 1;
            List<string> types = new List<string>{ "slide", "hall" };
            ViewData["LocationType"] = new SelectList(types);
            ViewData["LocationSlide"] = new SelectList(_context.Slides, "SlideId", "SlideId");
            ViewData["LocationHall"] = new SelectList(_context.Halls, "HallId", "HallId");
            ViewData["TicketOwner"] = new SelectList(_context.Visitors, "VisitorId", "VisitorId");
            string errors = "";
            ViewBag.ErrorString = errors;
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketId,LocationHall,LocationSlide,LocationType,TicketOwner,Price")] Ticket ticket)
        {
           
            ticket.TicketStatus = 1;
            if (ticket.LocationType == "slide")
            {
                ticket.LocationHall = null;
            }
            else if (ticket.LocationType == "hall")
            {
                ticket.LocationSlide = null;
            }

            byte errCheck = 0;
            
            if (ticket.LocationType == "hall")
            {
                int sum = _context.Tickets.Where(p => p.LocationHall == ticket.LocationHall && p.TicketStatus == 1).Count();
                Hall hall = _context.Halls.Where(h=>h.HallId==ticket.LocationHall).FirstOrDefault();
                if (sum >= hall.HallMaxPeople)
                {
                    ViewBag.ErrorString += " Забагато людей у обраній локації.";
                    errCheck++;
                }
                Visitor owner = _context.Visitors.Where(v=>v.VisitorId==ticket.TicketOwner).FirstOrDefault();
                var limits = from p in _context.Pools where p.Hall==ticket.LocationHall select p.PoolMinHeight;
                foreach (byte l in limits)
                    if (owner.Height < l) {
                        errCheck++;
                        ViewBag.ErrorString += " Зріст менший за мінімально дозволений ("+l+").";
                        break;
                    }
            }
            else
            {
                int sum = _context.Tickets.Where(p => p.LocationSlide == ticket.LocationSlide && p.TicketStatus == 1).Count();
                Slide slide = _context.Slides.Where(s => s.SlideId == ticket.LocationSlide).FirstOrDefault();
                if (sum >= slide.SlideMaxPeople)
                {
                    ViewBag.ErrorString += " Забагато людей у обраній локації.";
                    errCheck++;
                }
                Visitor owner = _context.Visitors.Where(v => v.VisitorId == ticket.TicketOwner).FirstOrDefault();
                Slide sl = _context.Slides.Where(s=>s.SlideId == ticket.LocationSlide).FirstOrDefault();
                if (owner.Height > sl.SlideMaxHeight)
                {
                    ViewBag.ErrorString += " Зріст більший за максимально дозволений ("+sl.SlideMaxHeight+").";
                    errCheck++;
                }

                if (owner.Height < sl.SlideMinHeight)
                {
                    ViewBag.ErrorString += " Зріст менший за мінімально дозволений ("+sl.SlideMinHeight+").";
                    errCheck++;
                }

                if (owner.Weight > sl.SlideMaxWeight)
                {
                    ViewBag.ErrorString += " Вага більша за максимально дозволену ("+sl.SlideMaxWeight+").";
                    errCheck++;
                }

                DateTime n = DateTime.Now; // To avoid a race condition around midnight
                int age = n.Year - owner.BirthDate.Year;

                if (n.Month < owner.BirthDate.Month || (n.Month == owner.BirthDate.Month && n.Day < owner.BirthDate.Day))
                    age--;
                if (age < sl.SlideMinAge)
                {
                    ViewBag.ErrorString += " Вік менший за мінімально дозволений ("+sl.SlideMinAge+").";
                    errCheck++;
                }
            }
            
            if (errCheck > 0)
            {
                List<string> types = new List<string> { "slide", "hall" };
                ViewData["LocationType"] = new SelectList(types);
                ViewData["LocationSlide"] = new SelectList(_context.Slides, "SlideId", "SlideId");
                ViewData["LocationHall"] = new SelectList(_context.Halls, "HallId", "HallId");
                ViewData["TicketOwner"] = new SelectList(_context.Visitors, "VisitorId", "VisitorId");
                return View(ticket);
            }

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
            List<string> types = new List<string> { "slide", "hall" };
            ViewBag.TicketId = ticket.TicketId;
            ViewBag.LocationType = ticket.LocationType;
            ViewBag.LocationHall = ticket.LocationHall;
            ViewBag.LocationSlide = ticket.LocationSlide;
            ViewBag.TicketOwner = ticket.TicketOwner;
            List<byte> status = new List<byte> { 0, 1 };
            ViewData["TicketStatus"] = new SelectList(status);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketId,LocationHall,LocationSlide, LocationType,TicketOwner,TicketStatus,Price")] Ticket ticket)
        {
            if (ticket.LocationType == "slide")
            {
                ticket.LocationHall = null;
            }
            else if (ticket.LocationType == "hall")
            {
                ticket.LocationSlide = null;
            }
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
