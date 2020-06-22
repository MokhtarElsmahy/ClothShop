using ClothShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothShop.ViewModels
{
    public class SearchViewModel
    {
        public List<Product> products { get; set; }
        public string searchTerm { get; set; }
        public int pageNo { get; set; }
    }
}