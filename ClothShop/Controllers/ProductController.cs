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
    public class ProductController : Controller
    {
        // GET: Product
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Category
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult ProductTable(string Search , int? pageNo)
        {
            SearchViewModel model = new SearchViewModel();
            model.searchTerm = Search; 
            int pageSize = 5;
            pageNo = pageNo ?? 1;
            model.pageNo = pageNo.Value;
            model.products = db.Products.ToList();//.OrderBy(p=>p.ProductID).Skip((pageNo.Value-1)*pageSize).Take(pageSize).Include(p=>p.Category).ToList();
            if (!string.IsNullOrEmpty(Search))
            {
                model.products = db.Products.Where(p => p.ProductName.ToLower().Contains(Search.ToLower())).OrderBy(p => p.ProductID).Skip((pageNo.Value - 1) * pageSize).Take(pageSize).Include(p => p.Category).ToList();
                return PartialView(model);
            }
            else
            {
                model.products = db.Products.OrderBy(p=>p.ProductID).Skip((pageNo.Value-1)*pageSize).Take(pageSize).Include(p=>p.Category).ToList();
                return PartialView(model);
            }
          
        }



        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            var product = db.Products.Find(id);
            return View(product);
        }

        // GET: Category/Create
        #region Create
        public ActionResult Create()
        {
            var categrories = db.Categories.ToList();
            return PartialView(categrories);
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                //product.Category = db.Categories.Find(product.CategoryID);
                // TODO: Add insert logic here
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("ProductTable");
            }
            else
            {
                return View(product);
            }
        } 
        #endregion

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            var product = db.Products.Find(id);

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);

            return PartialView(product);
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add update logic here
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProductTable");
            }
            else
            {
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
                return View(product);
            }
        }

        // GET: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("ProductTable");
        }

        // POST: Category/Delete/5
        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here
        //        var product = db.Products.Find(id);
        //        db.Products.Remove(product);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
