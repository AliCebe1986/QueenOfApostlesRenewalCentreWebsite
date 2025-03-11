using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using QueenOfApostlesRenewalCentre.Data;
using QueenOfApostlesRenewalCentre.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace QueenOfApostlesRenewalCentre.Controllers
{
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
        public async Task<IActionResult> Index(DateTime? arrival, DateTime? departure, int? guests)
        {
            var model = new BookViewModel
            {
                Arrival = arrival ?? DateTime.Today.AddDays(1),
                Departure = departure ?? DateTime.Today.AddDays(2),
                Guests = guests ?? 1,
                Rooms = 1, // Initialize Rooms property with default value
                OvernightOption = "No" // Default value
            };

            if (User.Identity?.IsAuthenticated ?? false)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    model.GuestName = $"{user.Name} {user.Surname}".Trim();
                    model.Email = user.Email ?? "";
                    model.PhoneNumber = user.PhoneNumber ?? "";
                }
            }

            await LoadAvailableRooms(model);

            return View(model);
        }

        // POST: /Book/CheckAvailability
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckAvailability(BookViewModel model)
        {
            if (model.Departure <= model.Arrival)
            {
                ModelState.AddModelError("Departure", "Departure date must be later than arrival date.");
            }

            if (model.Guests <= 0)
            {
                ModelState.AddModelError("Guests", "Number of guests must be at least 1.");
            }

            await LoadAvailableRooms(model);

            return View("Index", model);
        }

        // POST: /Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(BookViewModel model)
        {
            // Basic validation
            if (model.Departure <= model.Arrival)
            {
                ModelState.AddModelError("Departure", "Departure date must be later than arrival date.");
            }

            if (model.Guests <= 0)
            {
                ModelState.AddModelError("Guests", "Number of guests must be at least 1.");
            }

            if (string.IsNullOrEmpty(model.ReservationType))
            {
                ModelState.AddModelError("ReservationType", "Please select a reservation type.");
            }

            if (model.RoomId <= 0)
            {
                ModelState.AddModelError("RoomId", "Please select a valid room.");
            }
            else
            {
                // Make sure the room exists
                var room = await _context.Rooms.FindAsync(model.RoomId);
                if (room == null)
                {
                    ModelState.AddModelError("RoomId", "Selected room does not exist.");
                }
                else
                {
                    // Verify room availability
                    bool isRoomAvailable = await IsRoomAvailable(model.RoomId, model.Arrival, model.Departure);
                    if (!isRoomAvailable)
                    {
                        ModelState.AddModelError("RoomId", "This room is no longer available for the selected dates.");
                    }

                    // Verify room capacity
                    if (room.Capacity < model.Guests)
                    {
                        ModelState.AddModelError("RoomId", $"The selected room only accommodates {room.Capacity} guests.");
                    }
                }
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var booking = new Booking
                    {
                        GuestName = model.GuestName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        RoomId = model.RoomId,
                        StartDate = model.Arrival,
                        EndDate = model.Departure,
                        GuestCount = model.Guests,
                        Status = "Pending",
                        ReservationType = model.ReservationType,
                        UserId = _userManager.GetUserId(User),
                        BookingDate = DateTime.Now,
                        SpecialRequests = model.SpecialRequests
                    };

                    _context.Bookings.Add(booking);
                    await _context.SaveChangesAsync();

                    // Create invoice for the booking
                    await CreateInvoiceForBooking(booking);

                    return RedirectToAction("Confirmation", new { id = booking.BookingId });
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the exception
                Console.WriteLine($"Database update exception: {ex.Message}");
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message ?? "None"}");

                TempData["ErrorMessage"] = "There was a problem saving your booking. Please try again.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            // If we reach here, something failed, redisplay form
            await LoadAvailableRooms(model);
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

            // Ensure the booking belongs to the current user or user is admin
            var userId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");
            if (booking.UserId != userId && !isAdmin)
            {
                return Forbid();
            }

            // Get any related invoices
            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.BookingId == id);

            var model = new BookingConfirmationViewModel
            {
                Booking = booking,
                Invoice = invoice
            };

            return View(model);
        }

        // GET: /Book/MyBookings
        public async Task<IActionResult> MyBookings()
        {
            if (!(User.Identity?.IsAuthenticated ?? false))
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var userId = _userManager.GetUserId(User);
            var bookings = await _context.Bookings
                .Include(b => b.Room)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return View(bookings);
        }

        // GET: /Book/Cancel/5
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            // Ensure the booking belongs to the current user
            var userId = _userManager.GetUserId(User);
            if (booking.UserId != userId)
            {
                return Forbid();
            }

            // Can't cancel bookings that have already started
            if (booking.StartDate <= DateTime.Today)
            {
                TempData["Message"] = "Sorry, you cannot cancel a booking that has already started.";
                return RedirectToAction(nameof(MyBookings));
            }

            return View(booking);
        }

        // POST: /Book/Cancel/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            // Ensure the booking belongs to the current user
            var userId = _userManager.GetUserId(User);
            if (booking.UserId != userId)
            {
                return Forbid();
            }

            // Can't cancel bookings that have already started
            if (booking.StartDate <= DateTime.Today)
            {
                TempData["Message"] = "Sorry, you cannot cancel a booking that has already started.";
                return RedirectToAction(nameof(MyBookings));
            }

            // Update booking status to cancelled
            booking.Status = "Cancelled";
            _context.Update(booking);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Your booking has been successfully cancelled.";
            return RedirectToAction(nameof(MyBookings));
        }

        // Helper methods
        private async Task LoadAvailableRooms(BookViewModel model)
        {
            try
            {
                // Get all rooms
                var rooms = await _context.Rooms.ToListAsync();
                if (rooms == null || !rooms.Any())
                {
                    // No rooms available
                    model.AvailableRooms = new List<SelectListItem>();
                    return;
                }

                // Filter available rooms for the selected dates
                var availableRooms = new List<SelectListItem>();
                foreach (var room in rooms)
                {
                    bool isAvailable = await IsRoomAvailable(room.RoomId, model.Arrival, model.Departure);
                    if (isAvailable && room.Capacity >= model.Guests)
                    {
                        string showerInfo = room.WithShower ? "with shower" : "without shower";
                        availableRooms.Add(new SelectListItem
                        {
                            Value = room.RoomId.ToString(),
                            Text = $"{room.Name} - {room.RoomNumber} ({room.Capacity} persons, {showerInfo})"
                        });
                    }
                }

                model.AvailableRooms = availableRooms;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading rooms: {ex.Message}");
                model.AvailableRooms = new List<SelectListItem>();
            }
        }

        private async Task<bool> IsRoomAvailable(int roomId, DateTime startDate, DateTime endDate)
        {
            try
            {
                // Check if there are any overlapping bookings for this room
                return !await _context.Bookings
                    .Where(b => b.RoomId == roomId &&
                           b.Status != "Cancelled" &&
                           ((b.StartDate < endDate && b.EndDate > startDate))) // Overlapping logic
                    .AnyAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking room availability: {ex.Message}");
                return false; // Assume room is unavailable if there's an error
            }
        }

        private async Task CreateInvoiceForBooking(Booking booking)
        {
            try
            {
                // Get room price from database or use a default calculation
                var room = await _context.Rooms.FindAsync(booking.RoomId);
                decimal roomRate = 100.00m; // Default room rate if not available

                if (room != null)
                {
                    // You would typically have a pricing model in your database
                    roomRate = room.WithShower ? 120.00m : 100.00m;
                }

                // Calculate number of nights
                int numberOfNights = Math.Max(1, (int)(booking.EndDate - booking.StartDate).TotalDays);

                // Calculate total amount
                decimal totalAmount = roomRate * numberOfNights * booking.GuestCount;

                // Create the invoice
                var invoice = new Invoice
                {
                    BookingId = booking.BookingId,
                    Amount = totalAmount,
                    Status = "Unpaid",
                    IssuedDate = DateTime.Now
                };

                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating invoice: {ex.Message}");
                // Don't throw - we want to continue even if invoice creation fails
            }
        }
    }
}