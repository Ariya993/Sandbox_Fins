﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sandbox_Calc.Model
{
    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string? ProductName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? ProductImage { get; set; }
    }


}
