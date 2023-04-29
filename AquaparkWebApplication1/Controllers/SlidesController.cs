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
    public class SlidesController : Controller
    {
        private readonly AquaparkDbContext _context;

        public SlidesController(AquaparkDbContext context)
        {
            _context = context;
        }

        // GET: Slides
        public async Task<IActionResult> Index()
        {
              return _context.Slides != null ? 
                          View(await _context.Slides.ToListAsync()) :
                          Problem("Entity set 'AquaparkDbContext.Slides'  is null.");
        }

        // GET: Slides/Details/5
        public async Task<IActionResult> Details(byte? id)
        {
            if (id == null || _context.Slides == null)
            {
                return NotFound();
            }

            var slide = await _context.Slides
                .FirstOrDefaultAsync(m => m.SlideId == id);
            if (slide == null)
            {
                return NotFound();
            }
            //var owners = RedirectToAction("Tickets", "SlideInfo", slide.SlideId);
            return View(slide);
        }

        // GET: Slides/Create
        public IActionResult Create()
        {
            List<Slide> list = _context.Slides.ToList();
            int c = list.Count();
            if (c>0)
                ViewBag.SlideId = list.ElementAt(c - 1).SlideId + 1;
            else ViewBag.SlideId = 100;
            return View();
        }

        // POST: Slides/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SlideId,SlideMinHeight,SlideMaxHeight,SlideMaxWeight,SlideMaxPeople,SlideMinAge,SlideHighestPoint,SlideName,SlidePrice")] Slide slide)
        {
            if (ModelState.IsValid)
            {
                _context.Add(slide);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(slide);
        }

        // GET: Slides/Edit/5
        public async Task<IActionResult> Edit(byte? id)
        {
            if (id == null || _context.Slides == null)
            {
                return NotFound();
            }

            var slide = await _context.Slides.FindAsync(id);
            ViewBag.ErrorString = "";
            if (slide == null)
            {
                return NotFound();
            }
            return View(slide);
        }

        // POST: Slides/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(byte id, [Bind("SlideId,SlideMinHeight,SlideMaxHeight,SlideMaxWeight,SlideMaxPeople,SlideMinAge,SlideHighestPoint,SlideName,SlidePrice")] Slide slide)
        {
            if (id != slide.SlideId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var visitors = _context.Tickets.Where(t => t.TicketStatus == 1 && t.LocationSlide == slide.SlideId).ToList();
                    if (visitors.Any())
                    {
                        ViewBag.ErrorString += "Заборонено редагувати дані гірки за наявності відвідувачів. ";
                        return View(slide);
                    }
                    _context.Update(slide);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlideExists(slide.SlideId))
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
            return View(slide);
        }

        // GET: Slides/Delete/5
        public async Task<IActionResult> Delete(byte? id)
        {
            if (id == null || _context.Slides == null)
            {
                return NotFound();
            }

            var slide = await _context.Slides
                .FirstOrDefaultAsync(m => m.SlideId == id);
            if (slide == null)
            {
                return NotFound();
            }
            ViewBag.ErrorString = "";
            return View(slide);
        }

        // POST: Slides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(byte id)
        {
            if (_context.Slides == null)
            {
                return Problem("Entity set 'AquaparkDbContext.Slides'  is null.");
            }
            var slide = await _context.Slides.FindAsync(id);
            if (slide != null)
            {
                var visitors = _context.Tickets.Where(t => t.TicketStatus == 1 && t.LocationSlide == slide.SlideId).ToList();
                if (visitors.Any())
                {
                    ViewBag.ErrorString += "Заборонено видаляти гірки за наявності відвідувачів. ";
                    return View(slide);
                }
                //var relatedTickets = await _context.Tickets.Where(t => t.LocationSlide == id).ToListAsync();
                //int tId;
                //Ticket ticket;
                //while (relatedTickets.Any())
                //{
                //    tId = relatedTickets.FirstOrDefault().TicketId;
                //    ticket = _context.Tickets.FirstOrDefault(t => t.TicketId == tId);
                //    _context.Tickets.FirstOrDefault(_ => _.TicketId == tId).TicketStatus = 0;
                //    relatedTickets.Remove(ticket);
                //}
                //_context.Slides.Remove(slide);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SlideExists(byte id)
        {
          return (_context.Slides?.Any(e => e.SlideId == id)).GetValueOrDefault();
        }
    }
}
