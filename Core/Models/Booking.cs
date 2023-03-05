using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Booking
    {
        public int Id { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        [Required]
        public DateTime DateTo { get; set; }
        [Range(1,int.MaxValue)]
        public int BookedQuantity { get; set; }
        public int ResourceId { get; set; }
    }
}
