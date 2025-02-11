using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QueenOfApostlesRenewalCentre.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        // List of roles the user currently has (for display purposes)
        public IList<string> Roles { get; set; }

        // Role to assign during editing (single selection)
        public string SelectedRole { get; set; }
    }
}
