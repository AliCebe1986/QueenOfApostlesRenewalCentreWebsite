using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueenOfApostlesRenewalCentre.Data;
using QueenOfApostlesRenewalCentre.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QueenOfApostlesRenewalCentre.Controllers
{
    [Authorize] 
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get bookings and include related rooms
            var userBookings = await _context.Bookings
                                              .Where(b => b.UserId == userId)
                                              .ToListAsync();

            var allRoomIds = userBookings.SelectMany(b => b.RoomIds).Distinct().ToList();

            var allRooms = await _context.Rooms
                            .Where(r => allRoomIds.Contains(r.RoomId))
                            .ToListAsync();

            foreach (var booking in userBookings) {
                booking.Rooms = allRooms.Where(r => booking.RoomIds.Contains(r.RoomId)).ToList();
            }

            // Filter for upcoming and past bookings
            var upcoming = userBookings.Where(b => b.StartDate > DateTime.Now).ToList();
            var past = userBookings.Where(b => b.EndDate < DateTime.Now).ToList();

            // Create ViewModel with upcoming and past bookings
            var viewModel = new UserDashboardViewModel {
                UpcomingReservations = upcoming,
                PastReservations = past
            };

            return View(viewModel);
        }




        [HttpPost]
        public async Task<IActionResult> Cancel(int id) {
            var booking = await _context.Bookings
                                         .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null) {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (booking.UserId != userId) {
                return Unauthorized();
            }

            // Prevent cancellation if the booking start date has passed
            if (booking.StartDate <= DateTime.Now) {
                TempData["ErrorMessage"] = "Previous reservation cannot be cancelled.";
                return RedirectToAction(nameof(Index));
            }

            // Change the booking status to "Cancelled"
            booking.Status = "Cancelled";
            _context.Update(booking);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "The reservation was cancelled successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
