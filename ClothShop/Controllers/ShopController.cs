using ClothShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClothShop.ViewModels;
using System.Web.UI;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace ClothShop.Controllers
{
    public class ShopController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Shop


        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

      
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index(string searchTerm, int? minimumPrice, int? maximumPrice, int? CategoryID, int? sortBy, int? pageNo)
        {
            ShopViewModel model = new ShopViewModel();
            model.FeaturedCategories = db.Categories.Where(c => c.IsFeatured).ToList();
            model.MaximumPrice = (int)(db.Products.Max(p => p.ProductPrice));

            pageNo = pageNo ?? 1;
            model.SortBy = sortBy;
            model.CategoryID = CategoryID;
            model.SearchTerm = searchTerm;
            int totalCount = SearchProductsCount(searchTerm, minimumPrice, maximumPrice, CategoryID, sortBy);
            model.Products = SearchProducts(searchTerm, minimumPrice, maximumPrice, CategoryID, sortBy, pageNo.Value, 8);

            model.Pager = new Pager(totalCount, pageNo.Value, 8);

            return View(model);
        }

        public ActionResult FilterProducts(string searchTerm, int? minimumPrice, int? maximumPrice, int? CategoryID, int? sortBy, int? pageNo)
        {
            FilterProductsViewModel model = new FilterProductsViewModel();
            pageNo = pageNo ?? 1;
            model.SortBy = sortBy;
            model.CategoryID = CategoryID;
            model.SearchTerm = searchTerm;
            int totalCount = SearchProductsCount(searchTerm, minimumPrice, maximumPrice, CategoryID, sortBy);
            model.Products = SearchProducts(searchTerm, minimumPrice, maximumPrice, CategoryID, sortBy, pageNo.Value, 8);
            model.Pager = new Pager(totalCount, pageNo.Value, 8);






            return PartialView(model);
        }

        public List<Product> SearchProducts(string searchTerm, int? minimumPrice, int? maximumPrice, int? CategoryID, int? sortBy, int pageNo, int pageSize)
        {
            var products = db.Products.ToList();

            if (CategoryID.HasValue)
            {
                products = products.Where(p => p.CategoryID == CategoryID.Value).ToList();
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.ProductName.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
            else
            {
                products = products.ToList();
            }

            if (minimumPrice.HasValue)
            {
                products = products.Where(p => p.ProductPrice >= minimumPrice.Value).ToList();
            }
            if (maximumPrice.HasValue)
            {
                products = products.Where(p => p.ProductPrice <= maximumPrice.Value).ToList();
            }
            if (sortBy.HasValue)
            {
                switch (sortBy.Value)
                {

                    case 2:
                        products = products.OrderByDescending(p => p.ProductID).ToList();
                        break;
                    case 3:
                        products = products.OrderBy(p => p.ProductPrice).ToList();
                        break;
                    default:
                        products = products.OrderByDescending(p => p.ProductPrice).ToList();
                        break;

                }
            }

            return products.Skip((pageNo - 1) * pageSize).Take(pageSize)
                         .ToList();
        }

        public int SearchProductsCount(string searchTerm, int? minimumPrice, int? maximumPrice, int? CategoryID, int? sortBy)
        {
            var products = db.Products.ToList();

            if (CategoryID.HasValue)
            {
                products = products.Where(p => p.CategoryID == CategoryID.Value).ToList();
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.ProductName.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
            else
            {
                products = products.ToList();
            }

            if (minimumPrice.HasValue)
            {
                products = products.Where(p => p.ProductPrice >= minimumPrice.Value).ToList();
            }
            if (maximumPrice.HasValue)
            {
                products = products.Where(p => p.ProductPrice <= maximumPrice.Value).ToList();
            }
            if (sortBy.HasValue)
            {
                switch (sortBy.Value)
                {

                    case 2:
                        products = products.OrderByDescending(p => p.ProductID).ToList();
                        break;
                    case 3:
                        products = products.OrderBy(p => p.ProductPrice).ToList();
                        break;
                    default:
                        products = products.OrderByDescending(p => p.ProductPrice).ToList();
                        break;

                }
            }

            return products.Count;
        }
        public ActionResult Checkout()
        {
            CheckoutViewModel model = new CheckoutViewModel();
            var CartProductsCookie = Request.Cookies["CartProducts"];
            if (CartProductsCookie != null)
            {
                model.CartProductIDs = CartProductsCookie.Value.Split('-').Select(id => int.Parse(id)).ToList();
                model.CartProducts = db.Products.Where(p => model.CartProductIDs.Contains(p.ProductID)).ToList();

            }
            return View(model);
        }

        [Authorize]
        public ActionResult CheckoutUser()
        {
            CheckoutViewModel model = new CheckoutViewModel();
            var CartProductsCookie = Request.Cookies["CartProducts"];
            if (CartProductsCookie != null)
            {
                model.CartProductIDs = CartProductsCookie.Value.Split('-').Select(id => int.Parse(id)).ToList();
                model.CartProducts = db.Products.Where(p => model.CartProductIDs.Contains(p.ProductID)).ToList();
                model.User = UserManager.FindById(User.Identity.GetUserId());
                return View(model);

            }
            return RedirectToAction("Index");
        }

        public ActionResult PlaceOrder(string productIDs)
        {
            if (!string.IsNullOrEmpty(productIDs))
            {
                
                var CartProductIDs = productIDs.Split('-').Select(x => int.Parse(x)).ToList();
                var CartProducts =  db.Products.Where(product => CartProductIDs.Distinct().Contains(product.ProductID)).ToList();

                Order newOrder = new Order();
                newOrder.UserID = User.Identity.GetUserId();
                newOrder.OrderedAt = DateTime.Now;
                newOrder.Status = "Pending";
                newOrder.TotalAmount = CartProducts.Sum(x => x.ProductPrice * CartProductIDs.Where(productID => productID == x.ProductID).Count());

                newOrder.OrderItems = new List<OrderItem>();
                newOrder.OrderItems.AddRange(CartProducts.Select(product => new OrderItem() { ProductID = product.ProductID, Quantity = CartProductIDs.Where(productID => productID == product.ProductID).Count() }));

                db.Orders.Add(newOrder);
                var rowsEffected = db.SaveChanges();
                return Json(new { Success = true, Rows = rowsEffected }, JsonRequestBehavior.AllowGet);
               
            }
            else
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}