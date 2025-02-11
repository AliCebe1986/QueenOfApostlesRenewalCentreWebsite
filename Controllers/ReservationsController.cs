using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueenOfApostlesRenewalCentre.Data;
using QueenOfApostlesRenewalCentre.Models;
using Microsoft.EntityFrameworkCore;
using QueenOfApostlesRenewalCentre.Services;
using System.Security.Claims;

namespace QueenOfApostlesRenewalCentre.Controllers
{
    [Authorize]  
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public ReservationsController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["Rooms"] = _context.Rooms.ToList();
            return View();
        }

        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.Status = "Pending";
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                if (!string.IsNullOrEmpty(userEmail))
                {
                    var subject = "Reservation Confirmation";
                    var plainTextContent = $"Dear {booking.GuestName},\n\nYour reservation for room {booking.Room?.Name} from {booking.StartDate} to {booking.EndDate} has been received.";
                    var htmlContent = $"<p>Dear {booking.GuestName},</p><p>Your reservation for room <strong>{booking.Room?.Name}</strong> from <strong>{booking.StartDate}</strong> to <strong>{booking.EndDate}</strong> has been received.</p><p>Thank you for booking with us!</p>";

                    await _emailService.SendEmailAsync(userEmail, subject, plainTextContent, htmlContent);
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["Rooms"] = _context.Rooms.ToList();
            return View(booking);
        }

    }
}
