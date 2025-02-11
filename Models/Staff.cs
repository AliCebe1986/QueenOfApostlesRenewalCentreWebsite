using System.ComponentModel.DataAnnotations;

namespace QueenOfApostlesRenewalCentre.Models
{
    public class Staff
    {
        [Key]
        public int StaffId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        // "Admin", "Housekeeping", "Finance" etc..
        public string Role { get; set; }
    }
}
