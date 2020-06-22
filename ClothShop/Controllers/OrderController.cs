using ClothShop.Models;
using ClothShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace ClothShop.Controllers
{
    public class OrderController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Order
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
        public ActionResult Index(string UserID, string status, int? pageNo)
        {
            OrdersViewModel model = new OrdersViewModel();
            model.UserID = UserID;
            model.Status = status;

            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;
            var pageSize = 4;

            model.Orders = SearchOrders(UserID, status, pageNo.Value, pageSize);
            var totalRecords = SearchOrdersCount(UserID, status);

            model.Pager = new Pager(totalRecords, pageNo, pageSize);

            return View(model);
        }

        public List<Order> SearchOrders(string userID, string status, int pageNo, int pageSize)
        {
            using (var context = new ApplicationDbContext())
            {
                var orders = context.Orders.ToList();

                if (!string.IsNullOrEmpty(userID))
                {
                    orders = orders.Where(x => x.UserID.ToLower().Contains(userID.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(status))
                {
                    orders = orders.Where(x => x.Status.ToLower().Contains(status.ToLower())).ToList();
                }

                return orders.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
        }
        public int SearchOrdersCount(string userID, string status)
        {
            using (var context = new ApplicationDbContext())
            {
                var orders = context.Orders.ToList();

                if (!string.IsNullOrEmpty(userID))
                {
                    orders = orders.Where(x => x.UserID.ToLower().Contains(userID.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(status))
                {
                    orders = orders.Where(x => x.Status.ToLower().Contains(status.ToLower())).ToList();
                }

                return orders.Count;
            }
        }

        public ActionResult Details(int ID)
        {
            OrderDetailsViewModel model = new OrderDetailsViewModel();
            model.Order = db.Orders.Where(o=>o.ID==ID).Include(o => o.OrderItems).FirstOrDefault();
            if (model.Order != null)
            {
                model.OrderBy = UserManager.FindById(model.Order.UserID);
            }
            model.AvailableStatuses = new List<string>() { "Pending", "In Progress", "Delivered" };

            return View(model);
        }


        public ActionResult ChangeStatus(string status, int ID)
        {

            return Json(new { Success= UpdateOrderStatus(ID, status) },JsonRequestBehavior.AllowGet);
        }

        public bool UpdateOrderStatus(int ID, string status)
        {
            using (var context = new ApplicationDbContext())
            {
                var order = context.Orders.Find(ID);

                order.Status = status;

                context.Entry(order).State = EntityState.Modified;

                return context.SaveChanges() > 0;
            }
        }

    }
}