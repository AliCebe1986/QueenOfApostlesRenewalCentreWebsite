using System;
using System.ComponentModel.DataAnnotations;
using QueenOfApostlesRenewalCentre.Models;

namespace QueenOfApostlesRenewalCentre.Models {
    public class InvoiceViewModel {
        public int InvoiceId { get; set; }

        // Booking ViewModel instead of Booking directly
        public BookViewModel Booking { get; set; }

        // Room Charges Breakdown
        [DataType(DataType.Currency)]
        public decimal RoomCost { get; set; }

        // Meal Charges
        [DataType(DataType.Currency)]
        public decimal BreakfastCost { get; set; }

        [DataType(DataType.Currency)]
        public decimal LunchCost { get; set; }

        [DataType(DataType.Currency)]
        public decimal DinnerCost { get; set; }

        // Other Fees
        [DataType(DataType.Currency)]
        public decimal PremisesUseCost { get; set; }

        [DataType(DataType.Currency)]
        public decimal DirectorsDiscount { get; set; } // Can be a positive or negative value

        // Total Amount (calculated)
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }

        // Payment Details
        public string Status { get; set; } // "Paid", "Unpaid", "Overdue"

        [DataType(DataType.DateTime)]
        public DateTime IssuedDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DueDate { get; set; } // Optional: Helps track overdue status
    }
}
