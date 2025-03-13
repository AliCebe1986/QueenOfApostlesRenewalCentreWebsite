using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueenOfApostlesRenewalCentre.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public string GuestName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = ""; // Default değer eklendi

        [Phone]
        public string? PhoneNumber { get; set; } // Nullable yapıldı

        [ForeignKey("Room")]
        public int RoomId { get; set; }

        public Room? Room { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [Range(1, 20)]
        public int GuestCount { get; set; } = 1;

        // "Confirmed", "Pending", "Cancelled"
        public string Status { get; set; } = "Pending"; // Default değer eklendi

        public string ReservationType { get; set; } = ""; // Default değer eklendi

        public string? SpecialRequests { get; set; } // Nullable yapıldı

        public string? UserId { get; set; } // Nullable yapıldı

        [DataType(DataType.DateTime)]
        public DateTime BookingDate { get; set; } = DateTime.Now;
    }
}