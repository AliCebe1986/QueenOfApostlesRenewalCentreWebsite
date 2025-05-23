﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace QueenOfApostlesRenewalCentre.Models
{
    public class ApplicationUser : IdentityUser
    {
       
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        public bool loyalty { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
