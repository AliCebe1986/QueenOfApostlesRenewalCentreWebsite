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

        
        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public Room Room { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        //"Confirmed", "Pending", "Cancelled"
        public string Status { get; set; }

        public string ReservationType { get; set; }
        public string UserId { get; set; }
    }
}
