using ClothShop.Models;
using ClothShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClothShop.Controllers
{
    public class WidgetsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Widgets
        public ActionResult Products(bool isLatestProducts , int? CategoryID)
        {
            ProductsWidgetViewModel model = new ProductsWidgetViewModel();
            model.isLatestProducts = isLatestProducts;
            if (isLatestProducts)
            {
                model.Products = db.Products.OrderByDescending(p => p.ProductID).Take(4).Include(p=>p.Category).ToList();
               
            }
            else if (CategoryID.HasValue)
            {
                model.Products = db.Products.Where(p => p.Category.CategoryID==CategoryID.Value).Take(4).Include(p => p.Category).ToList();
            }
            else
            {
                int pageNo = 1;
                int pageSize = 11;
                model.Products = db.Products.OrderBy(p => p.ProductID).Skip((pageNo - 1) * pageSize).Take(pageSize).Include(p => p.Category).ToList();
                
            }
            return PartialView(model);
        }
    }
}