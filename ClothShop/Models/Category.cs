using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClothShop.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public string ImageURL { get; set; }
        public bool IsFeatured { get; set; }
        public virtual List<Product> Products { get; set; } 

    }
}