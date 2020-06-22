using ClothShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothShop.ViewModels
{
    public class ProductsWidgetViewModel
    {
        public List<Product> Products { get; set; }

        public bool isLatestProducts { get; set; }
    }
}