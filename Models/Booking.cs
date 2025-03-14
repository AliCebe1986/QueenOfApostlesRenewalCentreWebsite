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
        public string Email { get; set; } = ""; 

        [Phone]
        public string? PhoneNumber { get; set; }
      
        public List<int> RoomIds { get; set; } = new List<int>();

        public ICollection<Room> Rooms { get; set; } = new List<Room>();

        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [Range(1, 20)]
        public int GuestCount { get; set; } = 1;

        // "Confirmed", "Pending", "Cancelled"
        public string Status { get; set; } = "Pending"; 

        public string ReservationType { get; set; } = ""; 

        public string? SpecialRequests { get; set; } 

        public string? UserId { get; set; } 

        [DataType(DataType.DateTime)]
        public DateTime BookingDate { get; set; } = DateTime.Now;
    }
}