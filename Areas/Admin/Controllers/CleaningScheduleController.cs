using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueenOfApostlesRenewalCentre.Data;
using QueenOfApostlesRenewalCentre.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QueenOfApostlesRenewalCentre.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class CleaningScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CleaningScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/CleaningSchedule
        public async Task<IActionResult> Index()
        {
            var schedules = await _context.RoomCleanings.ToListAsync();
            return View(schedules);
        }

        // GET: Admin/CleaningSchedule/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/CleaningSchedule/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomCleaning schedule)
        {
            if (ModelState.IsValid)
            {
                _context.RoomCleanings.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(schedule);
        }

        // GET: Admin/CleaningSchedule/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var schedule = await _context.RoomCleanings.FindAsync(id);
            if (schedule == null)
                return NotFound();

            return View(schedule);
        }

        // POST: Admin/CleaningSchedule/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoomCleaning schedule)
        {
            if (id != schedule.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(schedule);
        }

        // GET: Admin/CleaningSchedule/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var schedule = await _context.RoomCleanings
                .FirstOrDefaultAsync(s => s.Id == id);
            if (schedule == null)
                return NotFound();

            return View(schedule);
        }

        // POST: Admin/CleaningSchedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _context.RoomCleanings.FindAsync(id);
            _context.RoomCleanings.Remove(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
            return _context.RoomCleanings.Any(e => e.Id == id);
        }
    }
}
