using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using QueenOfApostlesRenewalCentre.Data;
using QueenOfApostlesRenewalCentre.Models;
using Microsoft.EntityFrameworkCore;


[Authorize]
public class BookController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public BookController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: /Book
    public async Task<IActionResult> Index(DateTime? arrival, DateTime? departure, int? rooms, int? guests)
    {
        var model = new BookViewModel
        {
            Arrival = arrival ?? DateTime.Today,
            Departure = departure ?? DateTime.Today.AddDays(1),
            Rooms = rooms ?? 1,
            Guests = guests ?? 1
        };

        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                model.GuestName = $"{user.Name} {user.Surname}";
            }
        }

        model.AvailableRooms = await _context.Rooms
            .Select(r => new SelectListItem
            {
                Value = r.RoomId.ToString(),
                Text = $"{r.Name} (Capacity: {r.Capacity})"
            })
            .ToListAsync();
        

        return View(model);
    }

    // POST: /Book
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(BookViewModel model)
    {
        if (ModelState.IsValid)
        {
            var booking = new Booking
            {
                GuestName = model.GuestName,
                RoomId = model.RoomId,
                StartDate = model.Arrival,
                EndDate = model.Departure,
                Status = "Pending",
                ReservationType = model.ReservationType, // Save the selected reservation type
                UserId = _userManager.GetUserId(User)
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction("Confirmation", new { id = booking.BookingId });
        }

        model.AvailableRooms = await _context.Rooms
            .Select(r => new SelectListItem
            {
                Value = r.RoomId.ToString(),
                Text = $"{r.Name} (Capacity: {r.Capacity})"
            })
            .ToListAsync();

        return View(model);
    }

    // GET: /Book/Confirmation/5
    public async Task<IActionResult> Confirmation(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.Room)
            .FirstOrDefaultAsync(b => b.BookingId == id);

        if (booking == null)
        {
            return NotFound();
        }

        return View(booking);
    }
}
