using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClothShop.Models
{
    public class Config
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}