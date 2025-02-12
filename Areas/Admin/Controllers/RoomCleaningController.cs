using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using QueenOfApostlesRenewalCentre.Data;
using QueenOfApostlesRenewalCentre.Models;

namespace QueenOfApostlesRenewalCentre.Areas.Admin.Controllers
{
    public class RoomCleaningController : Controller
    {
        private readonly ApplicationDbContext _context;
        public RoomCleaningController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/RoomCleaning
        public async Task<IActionResult> Index(string searchString, DateTime? filterDate)
        {
            var cleanings = from rc in _context.RoomCleanings
                            select rc;

            // Search: RoomNumber and CleanerName
            if (!string.IsNullOrEmpty(searchString))
            {
                cleanings = cleanings.Where(rc => rc.RoomNumber.Contains(searchString) ||
                                                  rc.CleanerName.Contains(searchString));
            }

            // DepartureDate
            if (filterDate.HasValue)
            {
                cleanings = cleanings.Where(rc => rc.DepartureDate.Date == filterDate.Value.Date);
            }

            return View(await cleanings.ToListAsync());
        }

        // GET: Admin/RoomCleaning/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomCleaning = await _context.RoomCleanings.FirstOrDefaultAsync(m => m.Id == id);
            if (roomCleaning == null)
            {
                return NotFound();
            }

            return View(roomCleaning);
        }

        // GET: Admin/RoomCleaning/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/RoomCleaning/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomCleaning roomCleaning)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomCleaning);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(roomCleaning);
        }

        // GET: Admin/RoomCleaning/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var roomCleaning = await _context.RoomCleanings.FindAsync(id);
            if (roomCleaning == null)
            {
                return NotFound();
            }
            return View(roomCleaning);
        }

        // POST: Admin/RoomCleaning/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoomCleaning roomCleaning)
        {
            if (id != roomCleaning.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomCleaning);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomCleaningExists(roomCleaning.Id))
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
            return View(roomCleaning);
        }

        // GET: Admin/RoomCleaning/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var roomCleaning = await _context.RoomCleanings.FirstOrDefaultAsync(m => m.Id == id);
            if (roomCleaning == null)
            {
                return NotFound();
            }
            return View(roomCleaning);
        }

        // POST: Admin/RoomCleaning/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomCleaning = await _context.RoomCleanings.FindAsync(id);
            _context.RoomCleanings.Remove(roomCleaning);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomCleaningExists(int id)
        {
            return _context.RoomCleanings.Any(e => e.Id == id);
        }
    }
}
