using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Requests
{
    public class BookResourceRequest
    {
        [Required]
        public int ResourceId { get; set; }

        [Required]
        public DateTime DateFrom { get; set; }

        [Required]
        public DateTime DateTo { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The booked quantity must be at least 1")]
        public int BookedQuantity { get; set; }
    }
}
