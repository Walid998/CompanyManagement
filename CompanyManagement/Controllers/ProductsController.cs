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
    public class ProductsController : Controller
    {
        private CompanyManagmentEntities db = new CompanyManagmentEntities();

        public PartialViewResult SearchProduct(string ProductName)
        {
            var products = db.Products.Where(p => p.name.StartsWith(ProductName));
            return PartialView("_Products",products);
        }
        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.ToList();
            var Uoms = db.Unit_of_Measures.ToList();
            ProductViewModel productViewModel = new ProductViewModel()
            {
                Products = products,
                Uoms = Uoms
            };
            return View(productViewModel);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            var uoms = db.Unit_of_Measures.ToList();
            ProductViewModel productViewModel = new ProductViewModel() {
                Product = product,
                Uoms = uoms
            };
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(productViewModel);
        }
        // Create Product: Ajax
        [HttpPost]
        public JsonResult CreateProduct([Bind(Include = "name,vat,code,unit_of_measure")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return Json(product);
            }
            return null;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Edit([Bind(Include = "id,name,vat,code,unit_of_measure")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return Json(product);
            }
            return null;
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            try
            {
                db.Products.Remove(product);
                db.SaveChanges();
                return Json(true);
            }catch(DbUpdateException ex)
            {
                return null;
            }
        }

        // ============================================
        public ActionResult UoMCategories()
        {
            return View(db.UoMCategories.ToList());
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
