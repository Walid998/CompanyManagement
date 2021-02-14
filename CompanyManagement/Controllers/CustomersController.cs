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
    public class CustomersController : Controller
    {
        private CompanyManagmentEntities db = new CompanyManagmentEntities();

        
        // GET: Customers
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        } 

        public PartialViewResult SearchCustomers(string ClientName)
        {
            var customers = db.Customers.Where(c => c.name.StartsWith(ClientName)).ToList();
            return PartialView("_Customers", customers);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        // Create Customer: Ajax
        [HttpPost]
        public JsonResult CreateCustomer([Bind(Include = "name,phone,email,type")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return Json(customer);
            }
            return null;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Edit([Bind(Include = "id,name,phone,email,type")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return Json(customer);
            }
            return null;
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            try
            {
                db.Customers.Remove(customer);
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
