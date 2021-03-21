using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CompanyManagement.Models;

namespace CompanyManagement.Controllers
{
    public class UoMCategoriesController : Controller
    {
        private CompanyManagmentEntities db = new CompanyManagmentEntities();

        // GET: UoMCategories
        public ActionResult Index()
        {
            return View(db.UoMCategories.ToList());
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Create([Bind(Include = "name")] UoMCategory uoMCategory)
        {
            if (ModelState.IsValid)
            {
                db.UoMCategories.Add(uoMCategory);
                db.SaveChanges();
                return Json(uoMCategory);
            }
            return null;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Edit([Bind(Include = "id,name")] UoMCategory uoMCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uoMCategory).State = EntityState.Modified;
                db.SaveChanges();
                return Json(uoMCategory);
            }
            return null;
        }

        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            UoMCategory uoMCategory = db.UoMCategories.Find(id);
            try { 
                db.UoMCategories.Remove(uoMCategory);
                db.SaveChanges();
                return Json(true);
            }catch(DbUpdateException ex)
            {
                return null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
