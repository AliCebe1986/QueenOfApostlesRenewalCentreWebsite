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

        public async Task<IActionResult> Index()
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            
            var userBookings = await _context.Bookings
                                    .Include(b => b.Room)
                                    .Where(b => b.UserId == userId)
                                    .ToListAsync();

            
            var upcoming = userBookings.Where(b => b.StartDate > DateTime.Now).ToList();
            var past = userBookings.Where(b => b.EndDate < DateTime.Now).ToList();

            var viewModel = new UserDashboardViewModel
            {
                UpcomingReservations = upcoming,
                PastReservations = past
            };

            return View(viewModel);
        }

        
        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (booking.UserId != userId)
            {
                return Unauthorized();
            }

            
            if (booking.StartDate <= DateTime.Now)
            {
                TempData["ErrorMessage"] = "Geçmiş rezervasyonlar iptal edilemez.";
                return RedirectToAction(nameof(Index));
            }

            
            booking.Status = "Cancelled";
            _context.Update(booking);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Rezervasyon başarıyla iptal edildi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
