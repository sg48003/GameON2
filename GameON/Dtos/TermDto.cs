using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameON.Dtos
{
    public class TermDto
    {
        public int Id { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public bool Taken { get; set; }
        public int Price { get; set; }

        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }
        public string Place { get; set; }
        public int ZIPCode { get; set; }

        public int MinPeople { get; set; }
        public int MaxPeople { get; set; }

        //public ApplicationUser Owner { get; set; }
        public string OwnerId { get; set; }
    }
}