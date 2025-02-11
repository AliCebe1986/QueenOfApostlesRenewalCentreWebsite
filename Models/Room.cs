using System.ComponentModel.DataAnnotations;

namespace QueenOfApostlesRenewalCentre.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        [Required]
        [Display(Name = "Room Number")]
        public string RoomNumber { get; set; }

        [Required]
        public string Name { get; set; } 

        public int Capacity { get; set; }

        [Display(Name = "Room Type")]
        public string Type { get; set; }

        // True: With Shower, False: Without Shower
        [Display(Name = "Shower Option")]
        public bool WithShower { get; set; }

        // Indicates if the room is reserved today
        [Display(Name = "Reserved Today?")]
        public bool IsReserved { get; set; }
    }
}
