using System;
using System.ComponentModel.DataAnnotations;

namespace QueenOfApostlesRenewalCentre.Models
{
    public class WeeklySchedule
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

       
        public string GroupName { get; set; }

        public int NumberOfParticipants { get; set; }

        // "Breakfast", "Lunch", "Dinner", "Coffee Break"
        public string MealType { get; set; }

        
        public string ChapelBooking { get; set; }

        // "De Mazenod", "Smith", etc.)
        public string MeetingRoom { get; set; }

        
        public string CoffeeBreakTime { get; set; }

        
        public string Notes { get; set; }
    }
}
