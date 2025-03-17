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
    [Route("Book")]
    public class BookController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Book
        [HttpGet("Index")]
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

            model.AvailableRooms = await LoadAvailableRooms(model.Arrival, model.Departure, model.Guests);

            return View(model);
        }

        [HttpGet("SelectBookingType")]
        public IActionResult SelectBookingType() {
            return View();
        }

        [HttpPost("HandleBookingType")]
        public IActionResult HandleBookingType(string bookingType) {

            if (bookingType == "With") {
                return RedirectToAction("OvernightBooking");
            } else if (bookingType == "No") {
                return RedirectToAction("NoOvernightBooking");
            }

            TempData["ErrorMessage"] = "Please select a valid booking type.";
            return RedirectToAction("SelectBookingType");

        }

        [HttpGet("OvernightBooking")]
        public IActionResult OvernightBooking() {
            var model = new BookViewModel { OvernightOption = "With" };
            return View("OvernightBooking", model);
        }

        [HttpGet("NoOvernightBooking")]
        public IActionResult NoOvernightBooking() {
            var model = new BookViewModel { OvernightOption = "No" };
            return View("NoOvernightBooking", model);
        }


        

       
        [HttpGet("CheckAvailability")]
        public async Task<IActionResult> CheckAvailability([FromQuery] DateTime arrival, [FromQuery] DateTime departure, [FromQuery] int guests) {
            // Validation
            if (departure <= arrival) {
                return BadRequest("Departure date must be later than arrival date.");
            }

            if (guests <= 0) {
                return BadRequest("Number of guests must be at least 1.");
            }

            // Fetch available rooms
            var availableRooms = await LoadAvailableRooms(arrival, departure, guests);

            if (availableRooms == null || !availableRooms.Any()) {
                return NotFound("No available rooms for the selected dates.");
            }

            return Ok(availableRooms);
        }


        // POST: /Book

        [HttpPost("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(InvoiceViewModel model) {

            if (model.Booking.Departure <= model.Booking.Arrival) {
                ModelState.AddModelError("Departure", "Departure date must be later than arrival date.");
            }

            if (model.Booking.Guests <= 0) {
                ModelState.AddModelError("Guests", "Number of guests must be at least 1.");
            }

            if (model.Booking.RoomIds == null || model.Booking.RoomIds.Count == 0) {
                ModelState.AddModelError("Rooms", "Please select at least one room.");
            }

            // Check room availability and capacity
            var selectedRooms = await _context.Rooms
                .Where(r => model.Booking.RoomIds.Contains(r.RoomId))
                .ToListAsync();

            if (selectedRooms.Count != model.Booking.RoomIds.Count) {
                ModelState.AddModelError("RoomIds", "One or more selected rooms are invalid.");
            }

            var totalCapacity = selectedRooms.Sum(r => r.Capacity);
            if (totalCapacity < model.Booking.Guests) {
                ModelState.AddModelError("RoomIds", $"The selected rooms can only accommodate {totalCapacity} guests.");
            }

            // If validation fails, return to the correct view
            if (!ModelState.IsValid) {
                model.Booking.AvailableRooms = await LoadAvailableRooms(model.Booking.Arrival, model.Booking.Departure, model.Booking.Guests);

                if (model.Booking.OvernightOption == "With") {
                    return View("OvernightBooking", model.Booking);
                } else {
                    return View("NoOvernightBooking", model.Booking);
                }
            }

            // If validation passes, save booking
            var booking = new Booking {
                GuestName = model.Booking.GuestName,
                Email = model.Booking.Email,
                PhoneNumber = model.Booking.PhoneNumber,
                RoomIds = model.Booking.RoomIds,
                StartDate = model.Booking.Arrival,
                EndDate = model.Booking.Departure,
                GuestCount = model.Booking.Guests,
                Status = "Pending",
                ReservationType = model.Booking.ReservationType,
                UserId = _userManager.GetUserId(User),
                BookingDate = DateTime.Now,
                SpecialRequests = model.Booking.SpecialRequests
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            await CreateInvoiceForBooking(model, booking.BookingId);

            return RedirectToAction("Confirmation", new { id = booking.BookingId });
        }


        // GET: /Book/Confirmation/5
        [HttpGet("Confirmation/{id}")]
        public async Task<IActionResult> Confirmation(int id) {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == id);

            

            if (booking == null) {
                return NotFound();
            }

            if (booking.RoomIds != null && booking.RoomIds.Count > 0) {
                booking.Rooms = await _context.Rooms
                .Where(r => booking.RoomIds.Contains(r.RoomId))
                .ToListAsync();
            } else {
                booking.Rooms = new List<Room>();
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
        [HttpGet("MyBookings")]
        public async Task<IActionResult> MyBookings() {
            if (!(User.Identity?.IsAuthenticated ?? false)) {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var userId = _userManager.GetUserId(User);
            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            var allRoomIds = bookings.SelectMany(b => b.RoomIds).Distinct().ToList();
            var allRooms = await _context.Rooms
                .Where(r => allRoomIds.Contains(r.RoomId))
                .ToListAsync();

            foreach (var booking in bookings) {
                booking.Rooms = allRooms.Where(r => booking.RoomIds.Contains(r.RoomId)).ToList();
            }


            return View(bookings);
        }

        // GET: /Book/Cancel/5
        [HttpGet("Cancel/{id}")]
        public async Task<IActionResult> Cancel(int id) {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null) {
                return NotFound();
            }

            if (booking.RoomIds?.Count > 0) {
                booking.Rooms = await _context.Rooms
                    .Where(r => booking.RoomIds.Contains(r.RoomId))
                    .ToListAsync();
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
        [HttpPost("Cancel/{id}")]
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
        private async Task<List<SelectListItem>> LoadAvailableRooms(DateTime arrival, DateTime departure, int guests) {
            try {
                var rooms = await _context.Rooms.ToListAsync();

                var availableRooms = new List<SelectListItem>();
                int totalAvailableCapacity = 0;

                foreach (var room in rooms) {
                    bool isAvailable = await IsRoomAvailable(room.RoomId, arrival, departure);

                    if (isAvailable) {
                        string showerInfo = room.WithShower ? "with shower" : "without shower";
                        availableRooms.Add(new SelectListItem {
                            Value = room.RoomId.ToString(),
                            Text = $"{room.Name} - {room.RoomNumber} (2 guests, {showerInfo})"
                        });

                        totalAvailableCapacity += room.Capacity;
                    }
                }

                if (totalAvailableCapacity < guests) {
                    throw new Exception("The available rooms cannot accommodate the total number of guests.");
                }

                return availableRooms;
            } catch (Exception ex) {
                Console.WriteLine($"Error loading rooms: {ex.Message}");
                return new List<SelectListItem>(); // Return an empty list on error
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

        private async Task CreateInvoiceForBooking(InvoiceViewModel model, int bookingId) {

            try { 
           

                var invoice = new Invoice {
                    BookingId = bookingId,
                    RoomCost = model.RoomCost,
                    BreakfastCost = model.BreakfastCost,
                    LunchCost = model.LunchCost,
                    DinnerCost = model.DinnerCost,
                    PremisesUseCost = model.PremisesUseCost,
                    DirectorsDiscount = model.DirectorsDiscount,
                    TotalAmount = model.TotalAmount,
                    Status = "Unpaid",
                    IssuedDate = DateTime.Now,
                };

                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();
            } catch (Exception ex) {
                Console.WriteLine($"Error creating invoice: {ex.Message}");
            }
        }
      

        [HttpPost("CreateInvoice")]
        public async Task<IActionResult> CreateInvoice(BookViewModel bookViewModel) {


            if(ModelState.IsValid) {

                decimal roomCost = 0m;
                decimal breakfastCost = 0m;
                decimal lunchCost = 25.00m;
                decimal dinnerCost = 0m;
                decimal premisesUseCost = 35.00m;

                int numberOfNights = 1;

                if (bookViewModel.Departure != DateTime.MinValue) {

                     numberOfNights = Math.Max(1, (int)(bookViewModel.Departure - bookViewModel.Arrival).TotalDays);

                }

                if(bookViewModel.ReservationType != "HighSchool" && bookViewModel.ReservationType != "DayGroup") {
                    breakfastCost = 15.00m;
                    dinnerCost = 30.00m;
                }

               


                if (bookViewModel.OvernightOption != "No") {

                    if (bookViewModel.RoomIds?.Count == 0) {
                        return View("Error", new ErrorViewModel { Message = "No rooms selected for invoice." });
                    }

                    var selectedRooms = await _context.Rooms.Where(r => bookViewModel.RoomIds.Contains(r.RoomId)).ToListAsync();
                    foreach (var room in selectedRooms) {
                        decimal roomRate = room.WithShower ? 100.00m : 80.00m;
                        roomCost += roomRate * numberOfNights;
                    }

                    breakfastCost = breakfastCost * numberOfNights;

                    lunchCost = lunchCost * numberOfNights;

                    dinnerCost = dinnerCost * numberOfNights;

                    premisesUseCost = premisesUseCost * numberOfNights;

                }

               

                decimal totalCost = roomCost + ((breakfastCost + lunchCost + dinnerCost + premisesUseCost) * bookViewModel.Guests);

                Console.WriteLine(bookViewModel.RoomIds);


                var invoiceViewModel = new InvoiceViewModel {

                    Booking = bookViewModel,
                    RoomCost = roomCost,
                    BreakfastCost = breakfastCost,
                    LunchCost = lunchCost,
                    DinnerCost = dinnerCost,
                    PremisesUseCost = premisesUseCost,
                    DirectorsDiscount = 0,
                    TotalAmount = totalCost,
                    Status = "Unpaid",
                    IssuedDate = DateTime.Now,
                    DueDate = bookViewModel.Arrival,

                };

                Console.WriteLine(invoiceViewModel.Booking.RoomIds);

                return View("BookingConfirmation", invoiceViewModel);
            } else {
                return View("Error", new ErrorViewModel { Message = "Invoice generation failed"} );
            }


        }

    }
}
