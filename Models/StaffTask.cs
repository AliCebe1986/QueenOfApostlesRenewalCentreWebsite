using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueenOfApostlesRenewalCentre.Models
{
    public class StaffTask
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Assigned Date")]
        public DateTime AssignedDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Due Date")]
        public DateTime? DueDate { get; set; }

        // Örneğin: "Pending", "In Progress", "Completed"
        public string Status { get; set; }

        // Hangi staff'a atandığı (Foreign Key)
        [Required]
        public string StaffId { get; set; }

        [ForeignKey("StaffId")]
        public ApplicationUser Staff { get; set; }
    }
}
