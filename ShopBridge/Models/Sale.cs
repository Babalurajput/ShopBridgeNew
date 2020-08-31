using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ShopBridge.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        [StringLength(1000)]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [StringLength(1000)]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 999999999, ErrorMessage = "Price must be greater than 0.00")]
        [DisplayName("Price ($)")]
        public decimal Price { get; set; }
        public byte[] Image {get;set;}
    }
}