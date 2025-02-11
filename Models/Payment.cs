using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueenOfApostlesRenewalCentre.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [ForeignKey("Invoice")]
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        // "CreditCard", "PayPal", "Cash"
        public string PaymentMethod { get; set; }

        // "Completed", "Pending", "Failed"
        public string Status { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
    }
}
