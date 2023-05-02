using AquaparkWebApplication1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AquaparkWebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly AquaparkDbContext _context;
        public ChartController(AquaparkDbContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            List<object> locations = new List<object> { };
            var halls = _context.Halls.Where(h => h.Tickets.Where(t => t.TicketStatus == 1).Any()).ToList();
            var slides = _context.Slides.Where(s => s.Tickets.Where(t => t.TicketStatus == 1).Any()).ToList();
            var tickets = _context.Tickets.Where(t => t.TicketStatus == 1).ToList();
            locations.Add(new[] { "№", "Кількість відвідувачів" });
            foreach (var h in halls)
            {
                locations.Add(new object[] { h.HallId.ToString(), tickets.Where(t => t.LocationHall == h.HallId).Count() });
            }
            foreach (var s in slides)
            {
                locations.Add(new object[] { s.SlideId.ToString(), tickets.Where(t => t.LocationSlide == s.SlideId).Count() });
            }
            return new JsonResult(locations);
        }

        [HttpGet("JsonData1")]
        public JsonResult JsonData1()
        {
            List<object> locations = new List<object> { };
            var halls = _context.Halls.ToList();
            var slides = _context.Slides.ToList();
            locations.Add(new[] { "№", "Ціна квитка" });
            foreach (var h in halls)
            {
                locations.Add(new object[] { h.HallId.ToString(), h.HallPrice });
            }
            foreach (var s in slides)
            {
                locations.Add(new object[] { s.SlideId.ToString(), s.SlidePrice });
            }
            return new JsonResult(locations);
        }


    }
}
