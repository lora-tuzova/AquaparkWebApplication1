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
    public class PoolsController : Controller
    {
        private readonly AquaparkDbContext _context;

        public PoolsController(AquaparkDbContext context)
        {
            _context = context;
        }

        // GET: Pools
        public async Task<IActionResult> Index(byte? id)
        {
            if (id == null) return RedirectToAction("Halls", "Index");
            ViewBag.HallId = id;
            var PoolsByHall = _context.Pools.Where(p => p.Hall == id).Include(p => p.HallNavigation);
            return View(await PoolsByHall.ToListAsync());
        }

        // GET: Pools/Details/5
        public async Task<IActionResult> Details(byte? id)
        {
            if (id == null || _context.Pools == null)
            {
                return NotFound();
            }

            var pool = await _context.Pools
                .Include(p => p.HallNavigation)
                .FirstOrDefaultAsync(m => m.PoolId == id);
            if (pool == null)
            {
                return NotFound();
            }

            return View(pool);
        }

        // GET: Pools/Create
        public IActionResult Create(byte hallId)
        {
            ViewBag.HallId = hallId;
            List<Pool> list = _context.Pools.ToList();
            int c = list.Count();
            if (c > 0)
                ViewBag.PoolId = list.ElementAt(c - 1).PoolId + 1;
            else
                ViewBag.PoolId = 1;
            ViewBag.ErrorString = "";
            return View();
        }

        // POST: Pools/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PoolId,PoolDepth,PoolMinHeight,WaterType,Hall")] Pool pool)
        {

            Hall hall = _context.Halls.Where(h => h.HallId == pool.Hall).FirstOrDefault();
            if (hall == null || pool.PoolDepth < hall.PoolsMinDepth || pool.PoolDepth > hall.PoolsMaxDepth)
            {
                ViewBag.ErrorString += "Глибина басейна виходить за обмеження глибини цього хола ("+hall.PoolsMaxDepth+"-"+hall.PoolsMinDepth+").";
                ViewBag.HallId = pool.Hall;
                return View(pool);
            }
            if (ModelState.IsValid)
            {
                _context.Add(pool);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Halls");
            }
            
                //foreach (var modelState in ViewData.ModelState.Values)
                //{
                //    foreach (var error in modelState.Errors)
                //    {
                //        //Error details listed in var error
                //    }
                //}
            return View(pool);
        }

        // GET: Pools/Edit/5
        public async Task<IActionResult> Edit(byte? id)
        {
            if (id == null || _context.Pools == null)
            {
                return NotFound();
            }

            var pool = await _context.Pools.FindAsync(id);
            if (pool == null)
            {
                return NotFound();
            }
            ViewData["Hall"] = pool.Hall;
            List<byte> type = new List<byte> { 0, 1 };
            ViewData["WaterType"] = new SelectList(type);
            ViewBag.ErrorString = "";
            return View(pool);
        }

        // POST: Pools/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(byte id, [Bind("PoolId,PoolDepth,PoolMinHeight,WaterType,Hall")] Pool pool)
        {
            if (id != pool.PoolId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var visitors = _context.Tickets.Where(t => t.TicketStatus==1 && t.LocationHall==pool.Hall).ToList();
                    if (visitors.Any()){
                        ViewBag.ErrorString += "Заборонено редагувати дані басейна за наявності відвідувачів. ";
                        ViewData["Hall"] = new SelectList(_context.Halls, "HallId", "HallId", pool.Hall);
                        List<byte> type = new List<byte>{ 0, 1 };
                        ViewData["WaterType"] = new SelectList(type);
                        return View(pool);
                    }
                    _context.Update(pool);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PoolExists(pool.PoolId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Halls");
            }
            ViewData["Hall"] = pool.Hall;
            return View(pool);
            //return RedirectToAction("Index", "Halls");
        }

        // GET: Pools/Delete/5
        public async Task<IActionResult> Delete(byte? id)
        {
            if (id == null || _context.Pools == null)
            {
                return NotFound();
            }

            var pool = await _context.Pools
                .Include(p => p.HallNavigation)
                .FirstOrDefaultAsync(m => m.PoolId == id);
            if (pool == null)
            {
                return NotFound();
            }

            return View(pool);
        }

        // POST: Pools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(byte id)
        {
            if (_context.Pools == null)
            {
                return Problem("Entity set 'AquaparkDbContext.Pools'  is null.");
            }
            var pool = await _context.Pools.FindAsync(id);
            if (pool != null)
            {
                _context.Pools.Remove(pool);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Halls");
        }

        private bool PoolExists(byte id)
        {
            return (_context.Pools?.Any(e => e.PoolId == id)).GetValueOrDefault();
        }
    }
}
