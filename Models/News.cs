using System;
using System.ComponentModel.DataAnnotations;

namespace QueenOfApostlesRenewalCentre.Models
{
    public class News
    {
        [Key]
        public int NewsId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime PublishedDate { get; set; }

        // This property is used to manually control the order of news items.
        public int SortOrder { get; set; }
    }
}
