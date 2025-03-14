using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QueenOfApostlesRenewalCentre.Data;
using QueenOfApostlesRenewalCentre.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QueenOfApostlesRenewalCentre.Areas.Admin.Controllers {
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BookingsController : Controller {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: Admin/Bookings
        public async Task<IActionResult> Index() {
            var bookings = _context.Bookings.Include(b => b.Rooms); // Include Rooms in the query
            return View(await bookings.ToListAsync());
        }

        // GET: Admin/Bookings/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null)
                return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Rooms) // Include Rooms in the details view
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // GET: Admin/Bookings/Create
        public IActionResult Create() {
            // Populate ViewBag.Rooms for a multi-select dropdown (to select multiple rooms)
            ViewBag.Rooms = new SelectList(_context.Rooms, "RoomId", "Name");
            return View();
        }

        // POST: Admin/Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,GuestName,StartDate,EndDate,Status,ReservationType,UserId")] Booking booking, List<int> RoomIds) {
            if (ModelState.IsValid) {
                // Get the rooms selected by the user from the RoomIds
                booking.Rooms = _context.Rooms.Where(r => RoomIds.Contains(r.RoomId)).ToList();

                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate the room dropdown on error
            ViewBag.Rooms = new SelectList(_context.Rooms, "RoomId", "Name", RoomIds);
            return View(booking);
        }

        // GET: Admin/Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null)
                return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Rooms) // Include Rooms in the edit action
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
                return NotFound();

            // Pass selected rooms to the view for multi-select dropdown
            ViewBag.Rooms = new SelectList(_context.Rooms, "RoomId", "Name", booking.Rooms.Select(r => r.RoomId).ToList());
            return View(booking);
        }

        // POST: Admin/Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,GuestName,StartDate,EndDate,Status,ReservationType,UserId")] Booking booking, List<int> RoomIds) {
            if (id != booking.BookingId)
                return NotFound();

            if (ModelState.IsValid) {
                try {
                    // Get the rooms selected by the user from the RoomIds
                    booking.Rooms = _context.Rooms.Where(r => RoomIds.Contains(r.RoomId)).ToList();

                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!BookingExists(booking.BookingId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            // Repopulate the room dropdown on error
            ViewBag.Rooms = new SelectList(_context.Rooms, "RoomId", "Name", RoomIds);
            return View(booking);
        }

        // GET: Admin/Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null)
                return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Rooms) // Include Rooms in the delete action
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // POST: Admin/Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var booking = await _context.Bookings.FindAsync(id);
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id) {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
