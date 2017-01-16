using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameON.Models
{
    public class Term
    {
        public int Id { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy HH:mm}")]
        public DateTime StartTime { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy HH:mm}")]
        public DateTime EndTime { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy HH:mm}")]
        public DateTime Deadline { get; set; }

        public bool Taken { get; set; }
        public int? Price { get; set; }

        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }
        public string Place { get; set; }
        public int? ZIPCode { get; set; }

        [Required]
        public int MinPeople { get; set; }
        public int? MaxPeople { get; set; }

        public ApplicationUser Owner { get; set; }
        public string OwnerId { get; set; }
    }
}