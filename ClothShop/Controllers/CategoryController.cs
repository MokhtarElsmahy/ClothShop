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
    [Authorize(Roles ="admin")]
    public class CategoryController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        // GET: Category
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult CategoryTable(string Search, int? pageNo)
        {
            CategorySearchViewModel model = new CategorySearchViewModel();
            var categories = db.Categories.Include(x => x.Products).ToList();

            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;
            int pageSize = 3;
            int totalRecords = 0;

            if (!string.IsNullOrEmpty(Search))
            {
                model.SearchTerm = Search;
                totalRecords = categories.Where(c => c.CategoryName.ToLower().Contains(Search.ToLower())).ToList().Count;
                model.Categories = categories.Where(category => category.CategoryName != null &&
                         category.CategoryName.ToLower().Contains(Search.ToLower()))
                         .OrderBy(x => x.CategoryID).Skip((pageNo.Value - 1) * pageSize).Take(pageSize)
                         .ToList();
            }
            else
            {
                totalRecords = categories.Count;
                model.Categories = categories.OrderBy(c => c.CategoryID).Skip((pageNo.Value - 1) * pageSize).Take(pageSize).ToList();

            }
            if (model.Categories != null)
            {
                model.Pager = new Pager(totalRecords, pageNo, 3);

                return PartialView("CategoryTable", model);
            }
            else
            {
                return HttpNotFound();
            }



        }


       
        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Category/Create
        //[HttpPost]
        //public JsonResult Create(Category category)
        //{  استخدم دى زى ما هى كده مع اللى معموله كومنت ف الفيو
        //    if(ModelState.IsValid)
        //    {
        //        // TODO: Add insert logic here
        //        db.Categories.Add(category);
        //        db.SaveChanges();
        //        return Json(new {msg="Done" }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new { msg = "fail" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid) 
            {
                // TODO: Add insert logic here
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("CategoryTable");
            }
            else
            {
                return View(category);
            }
        }



        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            var category = db.Categories.Find(id);
            return PartialView(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add update logic here
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CategoryTable");
            }
            else
            {
                return View(category);
            }
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            var category = db.Categories.Find(id);
            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var category = db.Categories.Find(id);
                if (category.Products.Count > 0)
                    db.Products.RemoveRange(category.Products);
                db.Categories.Remove(category);
                db.SaveChanges();
                return RedirectToAction("CategoryTable");
            }
            catch
            {
                return View();
            }
        }
    }
}
