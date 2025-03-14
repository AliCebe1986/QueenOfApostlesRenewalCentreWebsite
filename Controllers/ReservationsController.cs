using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueenOfApostlesRenewalCentre.Data;
using QueenOfApostlesRenewalCentre.Models;
using Microsoft.EntityFrameworkCore;
using QueenOfApostlesRenewalCentre.Services;
using System.Security.Claims;

namespace QueenOfApostlesRenewalCentre.Controllers {
    [Authorize]
    public class ReservationsController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public ReservationsController(ApplicationDbContext context, EmailService emailService) {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeeklyBookings(int roomId, DateTime weekStartDate)
        {
            DateTime weekEndDate = weekStartDate.AddDays(6);

            var bookings = await _context.Bookings
                .Where(b => b.RoomIds.Contains(roomId) &&
                            b.Status != "Cancelled" &&
                            b.StartDate <= weekEndDate &&
                            b.EndDate >= weekStartDate)
                .ToListAsync();

            return Json(bookings);
        }


        // GET: Reservations/Create
        public IActionResult Create() {
            ViewData["Rooms"] = _context.Rooms.ToList();
            return View();
        }

        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking) {
            if (ModelState.IsValid) {
                // Fetch the selected room IDs from the form
                var selectedRoomIds = booking.RoomIds;

                // Ensure the selected rooms exist
                var selectedRooms = await _context.Rooms
                    .Where(r => selectedRoomIds.Contains(r.RoomId))
                    .ToListAsync();

                if (!selectedRooms.Any()) {
                    ModelState.AddModelError("RoomIds", "Please select at least one valid room.");
                    ViewData["Rooms"] = _context.Rooms.ToList();
                    return View(booking);
                }

                // Assign the selected rooms to the booking
                booking.Rooms = selectedRooms;
                booking.Status = "Pending";

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                // Generate room details for the email
                var roomDetails = string.Join(", ", selectedRooms.Select(r => $"{r.Name} (Room #{r.RoomNumber})"));

                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                if (!string.IsNullOrEmpty(userEmail)) {
                    var subject = "Reservation Confirmation";
                    var plainTextContent = $"Dear {booking.GuestName},\n\nYour reservation for the following rooms has been received:\n{roomDetails}\nFrom {booking.StartDate} to {booking.EndDate}.";
                    var htmlContent = $"<p>Dear {booking.GuestName},</p><p>Your reservation for the following rooms has been received:</p><ul>{string.Join("", selectedRooms.Select(r => $"<li>{r.Name} (Room #{r.RoomNumber})</li>"))}</ul><p>From <strong>{booking.StartDate}</strong> to <strong>{booking.EndDate}</strong>.</p><p>Thank you for booking with us!</p>";

                    await _emailService.SendEmailAsync(userEmail, subject, plainTextContent, htmlContent);
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["Rooms"] = _context.Rooms.ToList();
            return View(booking);
        }
    }
}
