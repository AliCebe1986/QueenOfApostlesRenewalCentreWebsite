using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueenOfApostlesRenewalCentre.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        [ForeignKey("Booking")]
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        // "Paid", "Unpaid", "Overdue"
        public string Status { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime IssuedDate { get; set; }
    }
}
