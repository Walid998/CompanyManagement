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
    public class Unit_of_MeasuresController : Controller
    {
        private CompanyManagmentEntities db = new CompanyManagmentEntities();

        // GET: Unit_of_Measures
        public ActionResult Index()
        {
            var unit_of_Measures = db.Unit_of_Measures.Include(u => u.UoMCategory);
            ViewBag.category_id = new SelectList(db.UoMCategories, "id", "name");

            return View(unit_of_Measures.ToList());
        }

        public PartialViewResult SearchUnits(string UnitName)
        {
            var Units = db.Unit_of_Measures.Where(u => u.unit_name.StartsWith(UnitName)).ToList();
            return PartialView("_Index",Units);
        }
        
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

            ViewBag.category_id = new SelectList(
                                db.UoMCategories, "id", "name", unit_of_Measures.UoMCategory.id);
            return View(unit_of_Measures);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Create([Bind(Include = "unit_name,category_id,unit_type,ratio")]
                                Unit_of_Measures unit_of_Measures)
        {
            if (ModelState.IsValid)
            {
                int? catId = unit_of_Measures.category_id;
                if(unit_of_Measures.unit_type == "reference" 
                    && db.UoMCategories.Find(catId).refranse_flag == true)
                {
                    return Json("يوجد بالفعل وحدة قياس مرجعية لهذه الفئة");
                }
                db.Unit_of_Measures.Add(unit_of_Measures);
                db.SaveChanges();
                return Json(unit_of_Measures);
            }
            return null;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Edit([Bind(Include = "id,unit_name,category_id,unit_type,ratio")]
                                Unit_of_Measures unit_of_Measures)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unit_of_Measures).State = EntityState.Modified;
                db.SaveChanges();
                return Json(unit_of_Measures);
            }
            return null;
        }

        
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            Unit_of_Measures unit_of_Measures = db.Unit_of_Measures.Find(id);
            try
            {
                db.Unit_of_Measures.Remove(unit_of_Measures);
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
