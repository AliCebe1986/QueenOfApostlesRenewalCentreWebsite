using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueenOfApostlesRenewalCentre.Data;
using QueenOfApostlesRenewalCentre.Models;

namespace QueenOfApostlesRenewalCentre.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/schedule")]
    [ApiController]
    public class ScheduleApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ScheduleApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/schedule/week → 
        [HttpGet("week")]
        public async Task<ActionResult<IEnumerable<WeeklySchedule>>> GetCurrentWeekSchedule()
        {
            var today = DateTime.Today;
            
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime monday = today.AddDays(-diff);
            DateTime sunday = monday.AddDays(6);

            var schedule = await _context.WeeklySchedules
                .Where(ws => ws.Date >= monday && ws.Date <= sunday)
                .ToListAsync();
            return schedule;
        }

        // GET: api/schedule/{id} 
        [HttpGet("{id}")]
        public async Task<ActionResult<WeeklySchedule>> GetScheduleEntry(int id)
        {
            var entry = await _context.WeeklySchedules.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }
            return entry;
        }

        // POST: api/schedule 
        [HttpPost]
        public async Task<ActionResult<WeeklySchedule>> CreateScheduleEntry(WeeklySchedule entry)
        {
            _context.WeeklySchedules.Add(entry);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetScheduleEntry), new { id = entry.Id }, entry);
        }

        // PUT: api/schedule/{id} 
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScheduleEntry(int id, WeeklySchedule entry)
        {
            if (id != entry.Id)
            {
                return BadRequest();
            }
            _context.Entry(entry).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.WeeklySchedules.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // DELETE: api/schedule/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScheduleEntry(int id)
        {
            var entry = await _context.WeeklySchedules.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }
            _context.WeeklySchedules.Remove(entry);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
