using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QueenOfApostlesRenewalCentre.Models
{
    public class BookViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Arrival")]
        public DateTime Arrival { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Departure")]
        public DateTime Departure { get; set; }

        [Required]
        [Range(1, 10)]
        [Display(Name = "Rooms")]
        public int Rooms { get; set; } = 1;

        [Required]
        [Range(1, 20)]
        [Display(Name = "Guests")]
        public int Guests { get; set; } = 1;

        [Required]
        [Display(Name = "Guest Name")]
        public string GuestName { get; set; } = "";

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Phone]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Select Room")]
        public int RoomId { get; set; }

        public IEnumerable<SelectListItem> AvailableRooms { get; set; } = new List<SelectListItem>();

        [Required]
        [Display(Name = "Overnight Option")]
        public string OvernightOption { get; set; } = "No";

        [Required]
        [Display(Name = "Reservation Type")]
        public string ReservationType { get; set; } = "";

        [Display(Name = "Special Requests")]
        public string? SpecialRequests { get; set; }
    }

    public class BookingConfirmationViewModel
    {
        public Booking Booking { get; set; } = null!;
        public Invoice? Invoice { get; set; }
    }
}