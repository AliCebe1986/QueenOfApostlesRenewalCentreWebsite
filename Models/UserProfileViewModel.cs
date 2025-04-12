using System.ComponentModel.DataAnnotations;

namespace QueenOfApostlesRenewalCentre.Models {
    public class UserProfileViewModel {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

    }
}