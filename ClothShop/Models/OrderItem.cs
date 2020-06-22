using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothShop.Models
{
    public class OrderItem
    {
        public int ID { get; set; }

        public int Quantity { get; set; }

        public int OrderID { get; set; }
        public virtual Order Order { get; set; }

        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
    }
}