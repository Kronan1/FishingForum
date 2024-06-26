﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FishingForum.Models
{
    public class SubCategory
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
