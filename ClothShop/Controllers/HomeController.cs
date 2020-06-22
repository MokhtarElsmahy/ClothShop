using ClothShop.Models;
using ClothShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClothShop.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();                                      //.OrderByDecending().take(4).ToList()
            model.FeaturedCategories = db.Categories.Where(c=>c.IsFeatured&&c.ImageURL!=null).ToList();
           
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}