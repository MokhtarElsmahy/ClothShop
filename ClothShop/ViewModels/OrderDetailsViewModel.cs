using ClothShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothShop.ViewModels
{
    public class OrderDetailsViewModel
    {
        public Order Order { get; set; }
        public ApplicationUser OrderBy { get; set; }
        public List<string> AvailableStatuses { get;  set; }
    }
}