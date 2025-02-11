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
        [Display(Name = "Rooms")]
        public int Rooms { get; set; }

        [Required]
        [Display(Name = "Guests")]
        public int Guests { get; set; }

        [Required]
        [Display(Name = "Guest Name")]
        public string GuestName { get; set; }

        [Required]
        [Display(Name = "Select Room")]
        public int RoomId { get; set; }

        public IEnumerable<SelectListItem> AvailableRooms { get; set; }

        // New properties for reservation type selection:
        [Required]
        [Display(Name = "Overnight Option")]
        public string OvernightOption { get; set; }  // Expected values: "No" or "With"

        [Required]
        [Display(Name = "Reservation Type")]
        public string ReservationType { get; set; }
    }
}
