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
    public class Unit_of_MeasuresController : Controller
    {
        private CompanyManagmentEntities db = new CompanyManagmentEntities();

        // GET: Unit_of_Measures
        public ActionResult Index()
        {
            var unit_of_Measures = db.Unit_of_Measures.Include(u => u.UoMCategory);
            return View(unit_of_Measures.ToList());
        }
        public PartialViewResult SearchUnits(string UnitName)
        {
            var Units = db.Unit_of_Measures.Where(u => u.unit_name.StartsWith(UnitName)).ToList();
            return PartialView("_Index",Units);
        }
        // GET: Unit_of_Measures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit_of_Measures unit_of_Measures = db.Unit_of_Measures.Find(id);
            if (unit_of_Measures == null)
            {
                return HttpNotFound();
            }
            return View(unit_of_Measures);
        }

        // GET: Unit_of_Measures/Create
        public ActionResult Create()
        {
            ViewBag.category_id = new SelectList(db.UoMCategories, "id", "name");
            return View();
        }

        // POST: Unit_of_Measures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,unit_name,category_id,unit_type,ratio")] Unit_of_Measures unit_of_Measures)
        {
            if (ModelState.IsValid)
            {
                db.Unit_of_Measures.Add(unit_of_Measures);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.category_id = new SelectList(db.UoMCategories, "id", "name", unit_of_Measures.category_id);
            return View(unit_of_Measures);
        }

        // GET: Unit_of_Measures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit_of_Measures unit_of_Measures = db.Unit_of_Measures.Find(id);
            if (unit_of_Measures == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.UoMCategories, "id", "name", unit_of_Measures.category_id);
            ViewBag.unit_type = unit_of_Measures.unit_type;
            return View(unit_of_Measures);
        }

        // POST: Unit_of_Measures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,unit_name,category_id,unit_type,ratio")] Unit_of_Measures unit_of_Measures)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unit_of_Measures).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.category_id = new SelectList(db.UoMCategories, "id", "name", unit_of_Measures.category_id);
            ViewBag.unit_type = new[] { "اكبر", "المرجعية", "اصغر" };
            return View(unit_of_Measures);
        }

        // GET: Unit_of_Measures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit_of_Measures unit_of_Measures = db.Unit_of_Measures.Find(id);
            if (unit_of_Measures == null)
            {
                return HttpNotFound();
            }
            return View(unit_of_Measures);
        }

        // POST: Unit_of_Measures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Unit_of_Measures unit_of_Measures = db.Unit_of_Measures.Find(id);
            db.Unit_of_Measures.Remove(unit_of_Measures);
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
