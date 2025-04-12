using System.ComponentModel.DataAnnotations;

namespace QueenOfApostlesRenewalCentre.Models {
    public class Contract {

        [Key]
        public int ContractId { get; set; }

        [Required]
        public int BookingId { get; set; }

        [Required]
        public string ContractName { get; set; }

        [Required]
        public byte[] ContractPdf { get; set; }

        [Required]
        public DateTime SignedAt { get; set; } = DateTime.Now;

        [Required]
        public string Status { get; set; }


    }
}
