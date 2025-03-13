using System.ComponentModel.DataAnnotations;

namespace QueenOfApostlesRenewalCentre.Models {
    public class ConferenceRoom {


        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }



    }
}
