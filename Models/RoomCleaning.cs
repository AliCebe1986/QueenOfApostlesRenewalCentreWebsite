using System;
using System.ComponentModel.DataAnnotations;

namespace QueenOfApostlesRenewalCentre.Models
{
    public class RoomCleaning
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Room Number")]
        public string RoomNumber { get; set; }

        [Display(Name = "Group Name")]
        public string GroupName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Departure Date")]
        public DateTime DepartureDate { get; set; }

        // E.g., "Pending", "In Progress", "Completed"
        [Display(Name = "Cleaning Status")]
        public string CleaningStatus { get; set; }

        [Display(Name = "Cleaner Name")]
        public string CleanerName { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Cleaning Date")]
        public DateTime? CleaningDate { get; set; }

        [Display(Name = "Additional Notes")]
        public string AdditionalNotes { get; set; }
    }
}
