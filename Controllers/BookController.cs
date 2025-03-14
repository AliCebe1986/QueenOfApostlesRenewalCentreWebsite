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

namespace QueenOfApostlesRenewalCentre.Controllers {
    [Authorize]
    public class BookController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Book
        public async Task<IActionResult> Index(DateTime? arrival, DateTime? departure, int? guests, int? rooms) {
            var model = new BookViewModel {
                Arrival = arrival ?? DateTime.Today.AddDays(1),
                Departure = departure ?? DateTime.Today.AddDays(2),
                Guests = guests ?? 1,
                Rooms = rooms ?? 1, // Initialize Rooms property with default value
                OvernightOption = "No" // Default value
            };

            if (User.Identity?.IsAuthenticated ?? false) {
                var user = await _userManager.GetUserAsync(User);
                if (user != null) {
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
        public async Task<IActionResult> CheckAvailability(BookViewModel model) {
            if (model.Departure <= model.Arrival) {
                ModelState.AddModelError("Departure", "Departure date must be later than arrival date.");
            }

            if (model.Guests <= 0) {
                ModelState.AddModelError("Guests", "Number of guests must be at least 1.");
            }

            await LoadAvailableRooms(model);

            return View("Index", model);
        }

        // POST: /Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(BookViewModel model) {
            if (model.Departure <= model.Arrival) {
                ModelState.AddModelError("Departure", "Departure date must be later than arrival date.");
            }

            if (model.Guests <= 0) {
                ModelState.AddModelError("Guests", "Number of guests must be at least 1.");
            }

            if (model.RoomIds == null || model.RoomIds.Count == 0) {
                ModelState.AddModelError("RoomIds", "Please select at least one room.");
            }

            // Check room availability and capacity
            var selectedRooms = await _context.Rooms
                .Where(r => model.RoomIds.Contains(r.RoomId))
                .ToListAsync();

            if (selectedRooms.Count != model.RoomIds.Count) {
                ModelState.AddModelError("RoomIds", "One or more selected rooms are invalid.");
            }

            var totalCapacity = selectedRooms.Sum(r => r.Capacity);
            if (totalCapacity < model.Guests) {
                ModelState.AddModelError("RoomIds", $"The selected rooms can only accommodate {totalCapacity} guests.");
            }

            if (ModelState.IsValid) {
                var booking = new Booking {
                    GuestName = model.GuestName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    RoomIds = model.RoomIds,
                    Rooms = selectedRooms,
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

                await CreateInvoiceForBooking(booking);

                return RedirectToAction("Confirmation", new { id = booking.BookingId });
            }

            await LoadAvailableRooms(model);
            return View(model);
        }

        // GET: /Book/Confirmation/5
        public async Task<IActionResult> Confirmation(int id) {
            var booking = await _context.Bookings
                .Include(b => b.Rooms)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null) {
                return NotFound();
            }

            // Ensure the booking belongs to the current user or user is admin
            var userId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");
            if (booking.UserId != userId && !isAdmin) {
                return Forbid();
            }

            // Get any related invoices
            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.BookingId == id);

            var model = new BookingConfirmationViewModel {
                Booking = booking,
                Invoice = invoice
            };

            return View(model);
        }

        // GET: /Book/MyBookings
        public async Task<IActionResult> MyBookings() {
            if (!(User.Identity?.IsAuthenticated ?? false)) {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var userId = _userManager.GetUserId(User);
            var bookings = await _context.Bookings
                .Include(b => b.Rooms)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return View(bookings);
        }

        // GET: /Book/Cancel/5
        public async Task<IActionResult> Cancel(int id) {
            var booking = await _context.Bookings
                .Include(b => b.Rooms)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null) {
                return NotFound();
            }

            // Ensure the booking belongs to the current user
            var userId = _userManager.GetUserId(User);
            if (booking.UserId != userId) {
                return Forbid();
            }

            // Can't cancel bookings that have already started
            if (booking.StartDate <= DateTime.Today) {
                TempData["Message"] = "Sorry, you cannot cancel a booking that has already started.";
                return RedirectToAction(nameof(MyBookings));
            }

            return View(booking);
        }

        // POST: /Book/Cancel/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelConfirmed(int id) {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) {
                return NotFound();
            }

            // Ensure the booking belongs to the current user
            var userId = _userManager.GetUserId(User);
            if (booking.UserId != userId) {
                return Forbid();
            }

            // Can't cancel bookings that have already started
            if (booking.StartDate <= DateTime.Today) {
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
        private async Task LoadAvailableRooms(BookViewModel model) {
            try {
                var rooms = await _context.Rooms.ToListAsync();

                var availableRooms = new List<SelectListItem>();
                int totalAvailableCapacity = 0;

                foreach (var room in rooms) {
                    bool isAvailable = await IsRoomAvailable(room.RoomId, model.Arrival, model.Departure);

                    // Check if the room is available
                    if (isAvailable) {
                        string showerInfo = room.WithShower ? "with shower" : "without shower";
                        availableRooms.Add(new SelectListItem {
                            Value = room.RoomId.ToString(),
                            Text = $"{room.Name} - {room.RoomNumber} (2 guests, {showerInfo})"
                        });

                        // Add the room's capacity to the total available capacity (assuming 2 guests per room)
                        totalAvailableCapacity += room.Capacity;
                    }
                }

                // Check if the total available capacity can accommodate the number of guests
                if (totalAvailableCapacity < model.Guests) {
                    ModelState.AddModelError("Guests", "The available rooms cannot accommodate the total number of guests.");
                }

                model.AvailableRooms = availableRooms;
            } catch (Exception ex) {
                Console.WriteLine($"Error loading rooms: {ex.Message}");
                model.AvailableRooms = new List<SelectListItem>();
            }
        }







        private async Task<bool> IsRoomAvailable(int roomId, DateTime startDate, DateTime endDate) {
            try {
                // Check if there are any overlapping bookings for this room
                return !await _context.Bookings
                    .Where(b => b.RoomIds.Contains(roomId) &&
                           b.Status != "Cancelled" &&
                           ((b.StartDate < endDate && b.EndDate > startDate))) // Overlapping logic
                    .AnyAsync();
            } catch (Exception ex) {
                Console.WriteLine($"Error checking room availability: {ex.Message}");
                return false; // Assume room is unavailable if there's an error
            }
        }

        private async Task CreateInvoiceForBooking(Booking booking) {
            try {
                var selectedRooms = await _context.Rooms
                    .Where(r => booking.RoomIds.Contains(r.RoomId))
                    .ToListAsync();

                decimal roomCost = 0m;
                int numberOfNights = Math.Max(1, (int)(booking.EndDate - booking.StartDate).TotalDays);

                foreach (var room in selectedRooms) {
                    decimal roomRate = room.WithShower ? 120.00m : 100.00m;
                    roomCost += roomRate * numberOfNights;
                }

                decimal breakfastCost = 15.00m;
                decimal lunchCost = 25.00m;
                decimal dinnerCost = 30.00m;
                decimal premisesUseCost = 35.00m;
                decimal directorDiscount = 0m;

             
                if (booking.ReservationType == "High Schools" && booking.ReservationType == "Group Retreat (One Day)") {

                   breakfastCost = 0m;
                   dinnerCost = 0m; 

                }

                decimal individualSubTotal = breakfastCost + lunchCost + dinnerCost + premisesUseCost;

                var user = await _userManager.FindByIdAsync(booking.UserId);

                               



                decimal totalAmount = roomCost + (individualSubTotal * booking.GuestCount);

                var invoice = new Invoice {
                    BookingId = booking.BookingId,
                    RoomCost = roomCost,
                    BreakfastCost = breakfastCost,
                    LunchCost = lunchCost,
                    DinnerCost = dinnerCost,
                    PremisesUseCost = premisesUseCost,
                    DirectorsDiscount = directorDiscount,
                    TotalAmount = totalAmount,
                    Status = "Unpaid",
                    IssuedDate = DateTime.Now,
                };

                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();
            } catch (Exception ex) {
                Console.WriteLine($"Error creating invoice: {ex.Message}");
            }
        }
    }
}
