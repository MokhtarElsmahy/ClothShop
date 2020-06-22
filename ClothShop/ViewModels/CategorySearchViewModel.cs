using ClothShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothShop.ViewModels
{
    public class CategorySearchViewModel
    {
        public List<Category> Categories { get; set; }
        public string SearchTerm { get; set; }
        public Pager Pager { get; set; }
    }
}