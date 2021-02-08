using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        // GET: UoMCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UoMCategory uoMCategory = db.UoMCategories.Find(id);
            if (uoMCategory == null)
            {
                return HttpNotFound();
            }
            return View(uoMCategory);
        }

        // GET: UoMCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UoMCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "name")] UoMCategory uoMCategory)
        {
            if (ModelState.IsValid)
            {
                db.UoMCategories.Add(uoMCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(uoMCategory);
        }

        // GET: UoMCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UoMCategory uoMCategory = db.UoMCategories.Find(id);
            if (uoMCategory == null)
            {
                return HttpNotFound();
            }
            return View(uoMCategory);
        }

        // POST: UoMCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "name")] UoMCategory uoMCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uoMCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(uoMCategory);
        }

        // GET: UoMCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UoMCategory uoMCategory = db.UoMCategories.Find(id);
            if (uoMCategory == null)
            {
                return HttpNotFound();
            }
            return View(uoMCategory);
        }

        // POST: UoMCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UoMCategory uoMCategory = db.UoMCategories.Find(id);
            db.UoMCategories.Remove(uoMCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
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
